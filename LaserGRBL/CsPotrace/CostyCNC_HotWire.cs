using System;
using System.Collections.Generic;
using System.Text;

namespace CsPotrace
{
	public partial class Potrace
	{
		// =========================================================================
		// 🛠️ FILE ESTERNO COSTYCNC - RIMOZIONE SCALINI E ZIG-ZAG SU ASSI X,Y
		// =========================================================================
		static Path bmToPathlist_EsternaFiloCaldo()
		{
			Bitmap_p bm1 = bm.copy();
			Point currentPoint = new Point(0, 0);
			Path path = new Path();

			// 1. Esecuzione standard dell'algoritmo nativo di Potrace
			bool weiter = findNext(bm1, currentPoint, ref currentPoint);
			while (weiter)
			{
				path = findPath(bm1, currentPoint);
				xorPath(bm1, path);

				if (path.area > turdsize)
				{
					pathlist.Add(path);
				}
				weiter = findNext(bm1, currentPoint, ref currentPoint);
			}

			// =========================================================================
			// 🚀 COSTRUZIONE FILE HTML - OTTIMIZZAZIONE ASSIALE DELLE LINEE (NO ZIG-ZAG)
			// =========================================================================
			if (pathlist != null && pathlist.Count > 0)
			{
				System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

				System.Text.StringBuilder html = new System.Text.StringBuilder();
				html.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'><title>CostyCNC Straight X,Y Lines</title></head>");
				html.AppendLine("<body style='margin:0; background:#111; color:#fff; overflow:hidden;'>");
				html.AppendLine($"<div style='background:#222; padding:10px; font-weight:bold; color:#00ffcc; font-family:sans-serif;'>[CostyCNC Debug] Straight X/Y Line Filter active. Zig-zag stairs removed!</div>");
				html.AppendLine("<canvas id='c' style='width:100vw; height:100vh; display:block;'></canvas>");
				
				// Esportiamo i contorni nativi in una struttura ad array di array per JavaScript
				html.AppendLine("<script>const originalContours = [");
				foreach (Path origPath in pathlist)
				{
					if (origPath.pt != null && origPath.pt.Count > 0)
					{
						html.Append("[");
						foreach (Point pnt in origPath.pt) 
						{ 
							html.Append($"{{x:{pnt.x},y:{pnt.y}}},"); 
						}
						html.Append("],");
					}
				}
				html.AppendLine("];");

				// Script JS per calcolare i confini, applicare il filtro ortogonale X/Y e disegnare
				html.AppendLine(@"
				const canvas = document.getElementById('c'); const ctx = canvas.getContext('2d');
				canvas.width = window.innerWidth; canvas.height = window.innerHeight;
				
				let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;
				originalContours.forEach(contour => {
					contour.forEach(pt => {
						if(pt.x < minX) minX = pt.x; if(pt.x > maxX) maxX = pt.x;
						if(pt.y < minY) minY = pt.y; if(pt.y > maxY) maxY = pt.y;
					});
				});

				const gw = maxX - minX, gh = maxY - minY;
				const scale = Math.min((canvas.width - 100) / (gw || 1), (canvas.height - 100) / (gh || 1));
				const ox = (canvas.width / 2) - ((minX + gw / 2) * scale); 
				const oy = (canvas.height / 2) - ((minY + gh / 2) * scale);

				originalContours.forEach(contour => {
					if(contour.length > 0) {
						
						// =========================================================================
						// ⚡ FILTRO COSTYCNC: COMPRIME GLI SCALINI ALLINEATI SU X E Y
						// =========================================================================
						const cleanContour = [];
						cleanContour.push(contour[0]);

						for (let i = 1; i < contour.length; i++) {
							let current = contour[i];
							
							if (cleanContour.length >= 2) {
								let prev = cleanContour[cleanContour.length - 1];
								let beforePrev = cleanContour[cleanContour.length - 2];

								// Se stiamo andando dritti su X (stessa Y) o dritti su Y (stessa X)
								// sovrascriviamo il punto intermedio estendendo la linea retta
								if ((beforePrev.x === prev.x && prev.x === current.x) || 
									(beforePrev.y === prev.y && prev.y === current.y)) {
									cleanContour[cleanContour.length - 1] = current; // Sostituisce e raddrizza
									continue;
								}
							}
							cleanContour.push(current);
						}
						// =========================================================================

						// 1. Disegna il contorno raddrizzato (Verde)
						ctx.lineWidth = 2.0;
						ctx.strokeStyle = '#33ff33';
						ctx.beginPath();
						ctx.moveTo(cleanContour[0].x * scale + ox, cleanContour[0].y * scale + oy);
						for(let j=1; j<cleanContour.length; j++) { 
							ctx.lineTo(cleanContour[j].x * scale + ox, cleanContour[j].y * scale + oy); 
						}
						ctx.closePath();
						ctx.stroke();

						// 2. Disegna un cerchio piccolo sul punto di ingresso (Rosso)
						const entryX = cleanContour[0].x * scale + ox;
						const entryY = cleanContour[0].y * scale + oy;
						
						ctx.beginPath();
						ctx.arc(entryX, entryY, 4, 0, 2 * Math.PI);
						ctx.fillStyle = '#ff3333';
						ctx.fill();
						ctx.strokeStyle = '#ffffff';
						ctx.lineWidth = 1;
						ctx.stroke();
					}
				});
				</script></body></html>");

				stopwatch.Stop();

				try { System.IO.File.WriteAllText("costycnc.html", html.ToString()); } catch {}
			}

			return path;
		}
	}
}

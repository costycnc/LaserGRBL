using System;
using System.Collections.Generic;
using System.Text;

namespace CsPotrace
{
	public partial class Potrace
	{
		// =========================================================================
		// 🛠️ FILE ESTERNO COSTYCNC - APERTURA SICURA (ZERO ALERT DI WINDOWS)
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
			// 🚀 COSTRUZIONE FILE HTML - MOSTRA I CONTORNI E I PUNTI DI INGRESSO
			// =========================================================================
			if (pathlist != null && pathlist.Count > 0)
			{
				System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

				System.Text.StringBuilder html = new System.Text.StringBuilder();
				html.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'><title>CostyCNC Path Entries View</title></head>");
				html.AppendLine("<body style='margin:0; background:#111; color:#fff; overflow:hidden;'>");
				html.AppendLine($"<div style='background:#222; padding:10px; font-weight:bold; color:#00ffcc; font-family:sans-serif;'>[CostyCNC Debug] Potrace generated {pathlist.Count} paths. Entry points highlighted in RED.</div>");
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

				// Script JS per calcolare i confini, scalare e disegnare i contorni + cerchietti di ingresso
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
						// 1. Disegna il contorno geometrico (Verde)
						ctx.lineWidth = 2.0;
						ctx.strokeStyle = '#33ff33';
						ctx.beginPath();
						ctx.moveTo(contour[0].x * scale + ox, contour[0].y * scale + oy);
						for(let j=1; j<contour.length; j++) { 
							ctx.lineTo(contour[j].x * scale + ox, contour[j].y * scale + oy); 
						}
						ctx.closePath();
						ctx.stroke();

						// 2. Disegna un cerchio piccolo sul punto di ingresso (Rosso)
						const entryX = contour[0].x * scale + ox;
						const entryY = contour[0].y * scale + oy;
						
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

				// =========================================================================
				// 🛡️ SOLUZIONE DEFINITIVA ANTI-ALERT: Scrittura in area Temp sicura di Windows
				// =========================================================================
				try 
				{ 
					// Genera un percorso univoco e sicuro nella cartella temporanea di sistema
					string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "costycnc_debug.html");
					
					// Scrive il file (Windows non blocca questa cartella perché è nata per questo)
					System.IO.File.WriteAllText(tempPath, html.ToString());

					// Apre il browser predefinito usando il file temporaneo appena creato
					System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
					{
						FileName = tempPath,
						UseShellExecute = true
					});
				} 
				catch {}
			}

			return path;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CsPotrace
{
	public partial class Potrace
	{
		// =========================================================================
		// 🛠️ FILE ESTERNO COSTYCNC - ROTAZIONE PRIMO PATH SUL PUNTO PIÙ VICINO A (0,0)
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
			// 🚀 COSTRUZIONE FILE HTML - LOGICA DI ROTAZIONE DEL PUNTO PIÙ VICINO A 0
			// =========================================================================
			if (pathlist != null && pathlist.Count > 0)
			{
				System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

				System.Text.StringBuilder html = new System.Text.StringBuilder();
				html.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'><title>CostyCNC Nearest to 0 Rotation</title></head>");
				html.AppendLine("<body style='margin:0; background:#111; color:#fff; overflow:hidden;'>");
				html.AppendLine($"<div style='background:#222; padding:10px; font-weight:bold; color:#00ffcc; font-family:sans-serif;'>[CostyCNC Debug] Step 1: First path rotated to the closest point to (0,0).</div>");
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

				// Script JS per calcolare i confini, ruotare il punto ideale e disegnare
				html.AppendLine(@"
				const canvas = document.getElementById('c'); const ctx = canvas.getContext('2d');
				canvas.width = window.innerWidth; canvas.height = window.innerHeight;
				
				// Copiamo la lista dei contorni per non alterare i dati di base durante i calcoli
				const processedContours = JSON.parse(JSON.stringify(originalContours));

				// =========================================================================
				// ⚡ TROVA IL PUNTO PIÙ VICINO A (0,0) TRA TUTTI I PATH E RUOTA QUEL PATH
				// =========================================================================
				let minDistanceSq = Infinity;
				let targetPathIndex = -1;
				let targetPointIndex = -1;

				// Cerchiamo in tutti i contorni e in tutti i loro punti
				for (let m = 0; m < processedContours.length; m++) {
					const contour = processedContours[m];
					for (let n = 0; n < contour.length; n++) {
						const pt = contour[n];
						// Calcoliamo la distanza al quadrato rispetto a X=0 e Y=0
						const distSq = (pt.x * pt.x) + (pt.y * pt.y);

						if (distSq < minDistanceSq) {
							minDistanceSq = distSq;
							targetPathIndex = m;   // Indice di quale sagoma è la più vicina
							targetPointIndex = n;  // Indice di quale punto di quella sagoma è il più vicino
						}
					}
				}

				// Se abbiamo trovato il punto più vicino, ruotiamo solo quella specifica sagoma
				if (targetPathIndex !== -1 && targetPointIndex !== -1) {
					const p = processedContours[targetPathIndex];
					
					// Eseguiamo la rotazione esatta sul punto 'targetPointIndex'
					const rotatedP = [];
					for (let k = targetPointIndex; k < p.length; k++) rotatedP.push(p[k]);
					for (let k = 0; k < targetPointIndex; k++) rotatedP.push(p[k]);
					
					// Sostituiamo la vecchia sagoma con quella ruotata
					processedContours[targetPathIndex] = rotatedP;
				}
				// =========================================================================

				// Calcolo ingombri standard per il bilanciamento dello schermo
				let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;
				processedContours.forEach(contour => {
					contour.forEach(pt => {
						if(pt.x < minX) minX = pt.x; if(pt.x > maxX) maxX = pt.x;
						if(pt.y < minY) minY = pt.y; if(pt.y > maxY) maxY = pt.y;
					});
				});

				const gw = maxX - minX, gh = maxY - minY;
				const scale = Math.min((canvas.width - 100) / (gw || 1), (canvas.height - 100) / (gh || 1));
				const ox = (canvas.width / 2) - ((minX + gw / 2) * scale); 
				const oy = (canvas.height / 2) - ((minY + gh / 2) * scale);

				// Disegno a schermo dei risultati
				processedContours.forEach((contour, idx) => {
					if(contour.length > 0) {
						// 1. Disegna la sagoma (Verde)
						ctx.lineWidth = 2.0;
						ctx.strokeStyle = '#33ff33';
						ctx.beginPath();
						ctx.moveTo(contour[0].x * scale + ox, contour[0].y * scale + oy);
						for(let j=1; j<contour.length; j++) { 
							ctx.lineTo(contour[j].x * scale + ox, contour[j].y * scale + oy); 
						}
						ctx.closePath();
						ctx.stroke();

						// 2. Disegna il cerchio sulla NUOVA entrata (Rosso)
						const entryX = contour[0].x * scale + ox;
						const entryY = contour[0].y * scale + oy;
						
						ctx.beginPath();
						ctx.arc(entryX, entryY, 4, 0, 2 * Math.PI);
						
						// Se questa è la sagoma che abbiamo modificato, facciamo il cerchietto azzurro 
						// per distinguerlo a colpo d'occhio, altrimenti lasciamolo rosso.
						ctx.fillStyle = (idx === targetPathIndex) ? '#00ffff' : '#ff3333';
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

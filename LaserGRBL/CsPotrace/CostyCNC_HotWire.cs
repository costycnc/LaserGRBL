using System;
using System.Collections.Generic;
using System.Text;

namespace CsPotrace
{
	// Estendiamo la classe parziale Potrace per accedere a bm, pathlist, findPath ecc.
	public partial class Potrace
	{
		// =========================================================================
		// 🛠️ FILE ESTERNO COSTYCNC - CODICE ATTIVO (PROVA BASE)
		// =========================================================================
		static Path bmToPathlist_EsternaFiloCaldo()
		{
			Bitmap_p bm1 = bm.copy();
			Point currentPoint = new Point(0, 0);
			Path path = new Path();

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

			// Restituiamo il tracciato base per verificare la stabilità iniziale del ponte
			return path;
		}
	}
}

// =========================================================================
// 📚 ARCHIVIO VECCHIO CODICE COMMENTATO (IGNORATO DAL COMPILATORE)
// =========================================================================
/*
		/// <summary>
		/// Decompose the given bitmap into paths. Returns a linked list of
		/// Path objects with the fields len, pt, area filled
		/// Optimized via Continuous Euclidean Nearest-Neighbor Tracking - CostyCNC Edition
		/// </summary>
		/// <param name="bm">A binary bitmap which holds the imageinformations.</param>
		/// <param name="plistp">List of Path objects</param>
		static Path bmToPathlist_VECCHIO_NON_FUNZIONANTE()
		{
			Bitmap_p bm1 = bm.copy();
			Point currentPoint = new Point(0, 0);
			Path path = new Path();

			// 1. Core Extraction Stage: Scan bitmap rows sequentially for raw path bounds
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
			// 🚀 VERO ALGORITMO GEOMETRICO CONTINUO - COSTYCNC ENGINE (DEBUG INGRESSI)
			// =========================================================================
			if (pathlist != null && pathlist.Count > 1)
			{
				System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
				int originalCount = pathlist.Count;

				// VARIABILE 2 (costyx): Converte la pathlist in una lista di liste di punti grezzi
				List<List<Point>> costyx = new List<List<Point>>();
				foreach (Path oldP in pathlist)
				{
					if (oldP.pt != null && oldP.pt.Count > 0)
					{
						costyx.Add(new List<Point>(oldP.pt));
					}
				}
				
				// VARIABILE 1 (pathx): Inizia con il punto di partenza (0,0)
				List<Point> pathx = new List<Point> { new Point(0, 0) };

				// CICLO PRINCIPALE EXTRACT.JS
				while (costyx.Count > 0)
				{
					double minDiff = double.MaxValue;
					int pos0 = 0;
					int pat1 = 0;
					int pos1 = 0;

					// I tre cicli for incrociati identici al tuo script (incremento di 10)
					for (int i = 0; i < pathx.Count; i += 10)
					{
						for (int m = 0; m < costyx.Count; m++)
						{
							List<Point> currentSegment = costyx[m];
							for (int n = 0; n < currentSegment.Count; n += 10)
							{
								double x = pathx[i].x;
								double y = pathx[i].y;
								double x1 = currentSegment[n].x;
								double y1 = currentSegment[n].y;

								double x2 = x - x1;
								double y2 = y - y1;
								double currDiff = (x2 * x2) + (y2 * y2);

								if (currDiff < minDiff)
								{
									minDiff = currDiff;
									pos0 = i;
									pat1 = m;
									pos1 = n;
								}
							}
						}
					}

					// Estraiamo il cammino più vicino
					List<Point> p = costyx[pat1];
					costyx.RemoveAt(pat1);

					// Rotazione esatta del cammino nel punto pos1
					List<Point> rotatedP = new List<Point>();
					for (int k = pos1; k < p.Count; k++) rotatedP.Add(p[k]);
					for (int k = 0; k < pos1; k++) rotatedP.Add(p[k]);
					p = rotatedP;

					// CHIUSURA CORRETTA: p.push(p[0]) di CostyCNC
					if (p.Count > 0)
					{
						p.Add(p[0]); 
					}

					// Unione continua delle fette
					List<Point> newPathx = new List<Point>();
					for (int k = 0; k <= pos0 && k < pathx.Count; k++) 
					{
						newPathx.Add(pathx[k]);
					}
					newPathx.AddRange(p);
					for (int k = pos0 + 1; k < pathx.Count; k++) 
					{
						newPathx.Add(pathx[k]);
					}

					pathx = newPathx;
				}

				// Metodo distruttivo controllato sulla pathlist centrale
				pathlist.Clear();
				Path finalCostyPath = new Path();
				finalCostyPath.pt = pathx;
				finalCostyPath.len = pathx.Count;
				finalCostyPath.area = 99999; 
				pathlist.Add(finalCostyPath);

				stopwatch.Stop();

				// Generazione file HTML autonomo
				System.Text.StringBuilder html = new System.Text.StringBuilder();
				html.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'><title>CostyCNC Step Logic</title></head>");
				html.AppendLine("<body style='margin:0; background:#111; color:#fff; overflow:hidden;'>");
				html.AppendLine($"<div style='background:#222; padding:10px; font-weight:bold; color:#00ffcc; font-family:sans-serif;'>[CostyCNC Debug] Loop Run completed in {stopwatch.ElapsedMilliseconds} ms.</div>");
				html.AppendLine("<canvas id='c' style='width:100vw; height:100vh; display:block;'></canvas>");
				html.AppendLine("<script>const pathLines = [");
				foreach (Point pnt in pathx) { html.Append($"{{x:{pnt.x},y:{pnt.y}}},"); }
				html.AppendLine("];");
				html.AppendLine(@"
				const canvas = document.getElementById('c'); const ctx = canvas.getContext('2d');
				canvas.width = window.innerWidth; canvas.height = window.innerHeight;
				let minX = Infinity, maxX = -Infinity, minY = Infinity, maxY = -Infinity;
				pathLines.forEach(pt => {
					if(pt.x < minX) minX = pt.x; if(pt.x > maxX) maxX = pt.x;
					if(pt.y < minY) minY = pt.y; if(pt.y > maxY) maxY = pt.y;
				});
				const gw = maxX - minX, gh = maxY - minY;
				const scale = Math.min((canvas.width - 100) / (gw || 1), (canvas.height - 100) / (gh || 1));
				const ox = (canvas.width / 2) - ((minX + gw / 2) * scale); const oy = (canvas.height / 2) - ((minY + gh / 2) * scale);
				ctx.beginPath(); ctx.strokeStyle = '#33ff33'; ctx.lineWidth = 2.5;
				if(pathLines.length > 0) {
					ctx.moveTo(pathLines.x * scale + ox, pathLines.y * scale + oy);
					for(let j=1; j<pathLines.length; j++) { ctx.lineTo(pathLines[j].x * scale + ox, pathLines[j].y * scale + oy); }
				}
				ctx.stroke();
				</script></body></html>");

				try { System.IO.File.WriteAllText("costycnc.html", html.ToString()); } catch {}
			}

			return path;
		}
*/

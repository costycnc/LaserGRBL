using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // Per l'oggetto Bitmap

namespace CsPotrace
{
	// Espandiamo la classe originale usando partial class
	public partial class Potrace
	{
		// =========================================================================
		// 🛠️ FUNZIONE DI ESTRAZIONE PUNTI GREZZI ED APERTURA HTML SICURA
		// =========================================================================
		static Path bmToPathlist_EsternaFiloCaldo()
		{
			Bitmap_p bm1 = bm.copy();
			Point currentPoint = new Point(0, 0);
			Path path = new Path();

			// 1. Estrazione standard dei contorni
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
			// 🚀 VISUALIZZATORE HTML DI CONTROLLO (Generato in area Temp sicura)
			// =========================================================================
			if (pathlist != null && pathlist.Count > 0)
			{
				System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
				StringBuilder html = new StringBuilder();
				html.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'><title>CostyCNC Real Time View</title></head>");
				html.AppendLine("<body style='margin:0; background:#111; color:#fff; overflow:hidden;'>");
				html.AppendLine("<canvas id='c' style='width:100vw; height:100vh; display:block;'></canvas>");
				html.AppendLine("<script>const originalContours = [");
				foreach (Path origPath in pathlist)
				{
					if (origPath.pt != null && origPath.pt.Count > 0)
					{
						html.Append("[");
						foreach (Point pnt in origPath.pt) { html.Append($"{{x:{pnt.x},y:{pnt.y}}},"); }
						html.Append("],");
					}
				}
				html.AppendLine("];");
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
				const ox = (canvas.width / 2) - ((minX + gw / 2) * scale); const oy = (canvas.height / 2) - ((minY + gh / 2) * scale);
				originalContours.forEach((contour, idx) => {
					if(contour.length > 0) {
						ctx.lineWidth = 2.0; ctx.strokeStyle = '#33ff33'; ctx.beginPath();
						ctx.moveTo(contour[0].x * scale + ox, contour[0].y * scale + oy);
						for(let j=1; j<contour.length; j++) { ctx.lineTo(contour[j].x * scale + ox, contour[j].y * scale + oy); }
						ctx.closePath(); ctx.stroke();
						ctx.beginPath(); ctx.arc(contour[0].x * scale + ox, contour[0].y * scale + oy, 4, 0, 2 * Math.PI);
						ctx.fillStyle = (idx === 0) ? '#00ffff' : '#ff3333'; // Azzurro per evidenziare il path modificato
						ctx.fill(); ctx.strokeStyle = '#ffffff'; ctx.lineWidth = 1; ctx.stroke();
					}
				});
				</script></body></html>");
				stopwatch.Stop();
				try {
					string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "costycnc_debug.html");
					System.IO.File.WriteAllText(tempPath, html.ToString());
					System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = tempPath, UseShellExecute = true });
				} catch {}
			}

			return path;
		}

		// =========================================================================
		// 🎯 NUOVA ELABORAZIONE POTRACETRACE (Sostituisce quella originale)
		// =========================================================================
		public static List<List<Curve>> PotraceTraceFiloCaldo(Bitmap Bitmap)
		{
			Clear();

			ConvertBitmap(Bitmap);
			
			// Eseguiamo l'estrazione e l'apertura dell'HTML
			bmToPathlist_EsternaFiloCaldo();

			// ⚡ ALGORITMO GEOMETRICO IN C#: TROVA IL PUNTO PIÙ VICINO A (0,0) E RUOTA IL PATH REALE
			if (pathlist != null && pathlist.Count > 0)
			{
				double minDistanceSq = double.MaxValue;
				int targetPathIndex = -1;
				int targetPointIndex = -1;

				// 1. Scansione matematica nella RAM dei punti per cercare il più vicino all'origine
				for (int m = 0; m < pathlist.Count; m++)
				{
					Path currentPath = pathlist[m];
					if (currentPath.pt != null)
					{
						for (int n = 0; n < currentPath.pt.Count; n++)
						{
							Point pt = currentPath.pt[n];
							double distSq = (pt.x * pt.x) + (pt.y * pt.y);

							if (distSq < minDistanceSq)
							{
								minDistanceSq = distSq;
								targetPathIndex = m;
								targetPointIndex = n;
							}
						}
					}
				}

				// 2. Ruotiamo l'array dei punti della sagoma identificata e la mettiamo in cima (Indice 0)
				if (targetPathIndex != -1 && targetPointIndex != -1)
				{
					Path targetPath = pathlist[targetPathIndex];
					List<Point> rotatedPoints = new List<Point>();

					for (int k = targetPointIndex; k < targetPath.pt.Count; k++) rotatedPoints.Add(targetPath.pt[k]);
					for (int k = 0; k < targetPointIndex; k++) rotatedPoints.Add(targetPath.pt[k]);

					targetPath.pt = rotatedPoints;
					targetPath.len = rotatedPoints.Count;

					// Spostiamo fisicamente questa sagoma all'indice 0 della pathlist per farla partire per prima
					if (targetPathIndex > 0)
					{
						pathlist.RemoveAt(targetPathIndex);
						pathlist.Insert(0, targetPath);
					}
				}
			}

			// Lasciamo che Potrace esegua lo smoothing standard per popolare temporaneamente la grafica di LaserGRBL
			for (int i = 0; i < pathlist.Count; i++)
			{
				calcSums(pathlist[i]);
				calcLon(pathlist[i]);
				bestPolygon(pathlist[i]);
				adjustVertices(pathlist[i]);
				smooth(pathlist[i]);
				if (curveoptimizing)
					optiCurve(pathlist[i]);
			}

			List<List<Curve>> rv = new List<List<Curve>>();
			tracetoList(rv);
			return rv;
		}
	}
}

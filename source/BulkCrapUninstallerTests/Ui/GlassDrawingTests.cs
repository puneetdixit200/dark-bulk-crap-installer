using System.Drawing;
using System.Drawing.Drawing2D;
using BulkCrapUninstaller.Controls.Theming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BulkCrapUninstallerTests.Ui
{
    [TestClass]
    public class GlassDrawingTests
    {
        [TestMethod]
        public void CreateRoundedRectangleKeepsInputBounds()
        {
            using var path = GlassDrawing.CreateRoundedRectangle(new Rectangle(4, 5, 120, 40), 8);
            AssertBoundsAreClose(new RectangleF(4, 5, 120, 40), path.GetBounds());
        }

        [TestMethod]
        public void CreateRoundedRectanglePreservesCurvedGeometry()
        {
            using var path = GlassDrawing.CreateRoundedRectangle(new Rectangle(4, 5, 120, 40), 8);

            Assert.IsTrue(path.PointCount > 4, "Rounded rectangle should contain curve points, not a flattened rectangle.");
            Assert.IsTrue(ContainsBezierPoint(path), "Rounded rectangle should preserve Bezier path data.");
        }

        [TestMethod]
        public void CreateRoundedRectangleFallsBackToRectangleForZeroRadius()
        {
            using var path = GlassDrawing.CreateRoundedRectangle(new Rectangle(0, 0, 20, 10), 0);
            Assert.AreEqual(new RectangleF(0, 0, 20, 10), path.GetBounds());
            Assert.AreEqual(4, path.PointCount);
        }

        [TestMethod]
        public void FillRoundedRectangleRestoresSmoothingMode()
        {
            using var bitmap = new Bitmap(12, 12);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.None;

            GlassDrawing.FillRoundedRectangle(graphics, new Rectangle(1, 1, 10, 10), Color.White, Color.Black, 3);

            Assert.AreEqual(SmoothingMode.None, graphics.SmoothingMode);
        }

        [TestMethod]
        public void FillGlassSurfaceRestoresSmoothingMode()
        {
            using var bitmap = new Bitmap(12, 12);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.None;

            GlassDrawing.FillGlassSurface(graphics, new Rectangle(1, 1, 10, 10), Color.White, elevated: true);

            Assert.AreEqual(SmoothingMode.None, graphics.SmoothingMode);
        }

        private static bool ContainsBezierPoint(GraphicsPath path)
        {
            foreach (var pointType in path.PathTypes)
            {
                if ((pointType & (byte)PathPointType.PathTypeMask) == (byte)PathPointType.Bezier)
                {
                    return true;
                }
            }

            return false;
        }

        private static void AssertBoundsAreClose(RectangleF expected, RectangleF actual)
        {
            const float tolerance = 0.01f;

            Assert.AreEqual(expected.X, actual.X, tolerance);
            Assert.AreEqual(expected.Y, actual.Y, tolerance);
            Assert.AreEqual(expected.Width, actual.Width, tolerance);
            Assert.AreEqual(expected.Height, actual.Height, tolerance);
        }
    }
}

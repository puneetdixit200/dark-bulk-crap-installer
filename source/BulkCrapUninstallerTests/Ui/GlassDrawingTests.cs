using System.Drawing;
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
            Assert.AreEqual(new RectangleF(4, 5, 120, 40), path.GetBounds());
        }

        [TestMethod]
        public void CreateRoundedRectangleFallsBackToRectangleForZeroRadius()
        {
            using var path = GlassDrawing.CreateRoundedRectangle(new Rectangle(0, 0, 20, 10), 0);
            Assert.AreEqual(new RectangleF(0, 0, 20, 10), path.GetBounds());
            Assert.AreEqual(4, path.PointCount);
        }
    }
}

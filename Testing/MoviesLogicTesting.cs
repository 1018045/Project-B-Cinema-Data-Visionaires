namespace Testing;

[TestClass]
public class TestMoviesLogic
{
    [TestMethod]
    public void PromoteMovieTestAdd()
    {
        MoviesLogic ml = new(new LogicManager());
        ml.PromoteMovie(new MovieModel(13000, "x", 10, 16, "x", new List<string>{}, "x"), 0);
        Assert.IsInstanceOfType<MovieModel>(ml.PromotedMovies[0]);
    }

    [TestMethod]
    public void PromoteMovieTestSetAsEmpty()
    {
        MoviesLogic ml = new(new LogicManager());
        ml.PromoteMovie(new MovieModel(13000, "x", 10, 16, "x", new List<string>{}, "x"), 0);
        ml.RemovePromotion(0);
        Assert.IsNull(ml.PromotedMovies[0]);
    }

    [TestMethod]
    [DataRow(-2, 3)]
    [DataRow(-1, 3)]
    [DataRow(0, 3)]
    [DataRow(1, 3)]
    [DataRow(2, 3)]
    [DataRow(3, 3)]
    [DataRow(4, 3)]
    public void PromoteMovieTestAddOutsideBoundaries(int position, int expectedSize)
    {
        MoviesLogic ml = new(new LogicManager());
        MovieModel mm = new MovieModel(13000, "x", 10, 16, "x", new List<string>{}, "x");
        ml.PromoteMovie(mm, position);
        if (position >= 0)
        {
            Assert.AreEqual(ml.PromotedMovies.Count, expectedSize);
        }
        else
        {
            // Assert just for checking no exception happened in the logic
            Assert.AreEqual(3, expectedSize);
        } 
    }

    [TestMethod]
    [DataRow(-2)]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    public void RemovePromotionTestAddOutsideBoundaries(int position)
    {
        MoviesLogic ml = new(new LogicManager());
        ml.RemovePromotion(position);
        if (position >= 0 && position <= 2)
        {
            Assert.IsNull(ml.PromotedMovies[position]);
        }
        else
        {
            // Assert just for checking no exception happened in the logic
            Assert.IsTrue(position < 0 || position > 2);
        } 
    }
}
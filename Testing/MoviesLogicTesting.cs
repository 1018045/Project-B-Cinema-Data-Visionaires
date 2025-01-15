namespace Testing;

[TestClass]
public class TestMoviesLogic
{
    [TestMethod]
    public void AddMovieTest()
    {
        LogicManager lm = new LogicManager();
        MoviesLogic ml = lm.MoviesLogic;
        ml.AddMovie("testAddMovie", 60, 14, "...", new(), "Ik");
        Assert.IsTrue(ml.Movies.Any(m => m.Title == "testAddMovie"));
    }


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
    public void RemovePromotionTest_AddOutsideBoundaries(int position)
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

    public void ArchiveMovieTest()
    {
        MoviesLogic ml = new(new LogicManager());
        MovieModel movieToArchive = new MovieModel(13000, "test", 60, 14, "testtest", new(), "Ik");

        ml.Movies.Add(movieToArchive);
        ml.ArchiveMovie(movieToArchive);

        Assert.IsFalse(ml.Movies.Contains(movieToArchive));
        Assert.IsTrue(ml.ArchivedMovies.Contains(movieToArchive));
    }

    public void ArchiveMovieTest_Null()
    {
        MoviesLogic ml = new(new LogicManager());
        
        int expected = ml.ArchivedMovies.Count;
        ml.ArchiveMovie(null);
        int actual = ml.ArchivedMovies.Count;

        Assert.AreEqual(expected, actual);
    }

    public void BringMovieBackFromArchiveTest()
    {
        MoviesLogic ml = new(new LogicManager());
        MovieModel movieToArchive = new MovieModel(13000, "test", 60, 14, "testtest", new(), "Ik");

        ml.ArchivedMovies.Add(movieToArchive);
        ml.BringMovieBackFromArchive(movieToArchive);

        Assert.IsFalse(ml.ArchivedMovies.Contains(movieToArchive));
        Assert.IsTrue(ml.Movies.Contains(movieToArchive));
    }

    public void BringMovieBackFromArchiveTest_Null()
    {
        MoviesLogic ml = new(new LogicManager());
        
        int expected = ml.Movies.Count;
        ml.BringMovieBackFromArchive(null);
        int actual = ml.Movies.Count;

        Assert.AreEqual(expected, actual);        
    }

    public void ArchiveMovieTest_HasUpcomingShowing()
    {
        LogicManager lm = new();
        MoviesLogic ml = lm.MoviesLogic;
        ShowingsLogic sl = lm.ShowingsLogic;

        MovieModel movieToArchive = new MovieModel(13000, "test", 60, 14, "testtest", new(), "Ik");
        sl.AddShowing(13000, DateTime.Now.AddDays(2), 1, 0, false, "", new());

        ml.Movies.Add(movieToArchive);
        Assert.IsFalse(ml.ArchiveMovie(movieToArchive));
    }
}
[System.Serializable]
public class LevelData
{
    public int numberOfTubes; // Number of tubes in the level
    public int ballsPerTube;  // Number of balls per tube
    public float tubeRadius;   // Radius of tube placement

    // Constructor to easily initialize levels
    public LevelData(int numTubes, int balls, float radius)
    {
        numberOfTubes = numTubes;
        ballsPerTube = balls;
        tubeRadius = radius;
    }
}
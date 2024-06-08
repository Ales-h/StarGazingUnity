using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarField : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float starSizeMin = 0.5f;
    [Range(0, 100)]
    [SerializeField] private float starSizeMax = 15f;
    [SerializeField] private float animationDuration = 1f; // délka animace pro úsečky konstelace

    private List<StarDataLoader.Star> stars;
    private List<GameObject> starObjects;
    private Dictionary<int, GameObject> constellationVisible = new();
    private readonly int starFieldScale = 800;
    


    void Start()
    {
        // Read in the star data.
        StarDataLoader sdl = new();
        stars = sdl.LoadData();
        starObjects = new();
      
        Material starmaterial = Resources.Load("starMaterial", typeof(Material)) as Material;
        starmaterial.shader = Shader.Find("Unlit/StarShader");

        foreach (StarDataLoader.Star star in stars)
        {
            
                GameObject stargo = GameObject.CreatePrimitive(PrimitiveType.Quad);
                stargo.transform.parent = transform;
                stargo.name = $"HR {star.catalog_number}";
                stargo.transform.localPosition = star.position * starFieldScale;
                stargo.transform.localScale = Vector3.one * Mathf.Lerp(starSizeMin, starSizeMax, star.size);
                //stargo.transform.LookAt(transform.position);
                //stargo.transform.Rotate(0, 100, 0);
                stargo.transform.LookAt(2 * stargo.transform.position - transform.position); // Properly faces the camera


                //Material material = stargo.GetComponent<MeshRenderer>().material;


                Renderer renderer = stargo.GetComponent<Renderer>();
                renderer.material = new Material(starmaterial); // nová instance materialu pro tento gameObject
                renderer.material.color = star.color; // nastavení barvy pro tuto hvězdu
                                                      //renderer.material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, star.size));

                starObjects.Add(stargo);
            if (star.position == new Vector3(1, 0, 0))
            {
                stargo.SetActive(false);
            }



            }

        
    }

    // zavolá se při změně v editoru, aby se updatenuli parametry hvězd 
    private void OnValidate()
    {
        if(starObjects!= null)
        {
            for(int i = 0; i < starObjects.Count; i++)
            {
                Material material = starObjects[i].GetComponent<MeshRenderer>().material;
                material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, stars[i].size));
            }
        }
    }



    private readonly List<(int[], int[], int, string, Vector3, int, int)> constellations = new()
    {
        // Orion

        // seznam indexu hvězd souhvezdí
        (new int[] { 1948, 1903, 1852, 2004, 1713, 2061, 1790, 1907, 2124,
            2199, 2135, 2047, 2159, 1543, 1544, 1570, 1552, 1567 },
        // seznam dvojic hvězd pro vytvoření úseček
     new int[] { 1713, 2004, 1713, 1852, 1852, 1790, 1852, 1903, 1903, 1948,
         1948, 2061, 1948, 2004, 1790, 1907, 1907, 2061, 2061, 2124,
         2124, 2199, 2199, 2135, 2199, 2159, 2159, 2047, 1790, 1543,
         1543, 1544, 1544, 1570, 1543, 1552, 1552, 1567, 2135, 2047 },
        1903, "Orion", new Vector3(0, 0, 0), 300, 35), // ¨hlavní¨ hvězda od které se počíta vzdálenost kurzoru pro zobrazení, název souhvězdí, offset textu od h. hvězdy, šířka canvasu
        (new int[] { 5191, 5062, 4905, 4660, 4554, 4295, 4518, 4301, 3757, 3323,
            3775, 3594, 3888 },

        new int[] { 5191, 5062, 4301, 4295, 5062, 4905, 4905, 4660, 4660, 4301, 4301, 3757, 3757, 3323, 3757,
            3888, 3323, 3888, 3888, 3775, 3775, 3594, 3888, 4295, 4295, 4554,
            4554, 4518, 4554, 4660 },

        4295, "Ursa Major", new Vector3(0, 0, 0), 200, 35),

        (new int[] { 2990, 2905, 2985, 2777, 2650, 2421, 2763, 2484, 2821, 2891, 2697, 2540, 2473, 2343, 2286, 2216 },

        new int[] { 2484, 2763, 2763, 2777, 2421, 2650, 2650, 2777, 2777, 2905, 2905, 2985, 2905, 2990, 2905, 2821,
            2891, 2697, 2697, 2540, 2697, 2821, 2697, 2473, 2473, 2343, 2473, 2286, 2286, 2216 },
        2821, "Gemini", new Vector3(0, 0, 0), 200, 35),

        (new int[] { 1910, 1457, 1411, 1346, 1239, 1030, 1373, 1178, 1339, 1409, 1497, 1791 },
        new int[] { 1910, 1457, 1457, 1411, 1411, 1346, 1346, 1239, 1239, 1030, 1346, 1373, 1373, 1178, 1373, 1339,
            1339, 1409, 1409, 1497, 1497, 1791 },
        1373, "Taurus", new Vector3(0, 0, 0), 200, 35),

        (new int[] { 7581, 7337, 7348, 7623, 7624, 7597, 7362, 7292, 7234, 7194, 7121, 7039, 6913, 6859, 6742, 6879, 6832, 7217, 7264, 7340, 6812 },
        new int[] { 7348, 7581, 7337, 7581, 7581, 7623, 7623, 7624, 7624, 7597, 7597, 7362, 7362, 7292, 7292, 7234, 7234, 7194, 7234, 7121,
            7039, 7194, 7121, 7217, 7217, 7264, 7264, 7340, 7039, 6913, 6913, 6812, 7039, 6859, 6859, 6879,
            6859, 6742, 6742, 6879, 6879, 6832, 6879, 7194, 6913, 6859 },
        7194, "Sagittarii", new Vector3(0, 0, 0), 400, 35),

        (new int[] { 4731, 4763, 4853, 4656 },
        new int[] { 4731, 4763, 4853, 4656 },
        4853, "Crux", new Vector3(0, 10, 40), 200, 20),

        (new int[] { 3475, 3449, 3369, 3262, 3461, 3510, 3572, 3249 },
        new int[] { 3475, 3449, 3262, 3369, 3369, 3449, 3449, 3461, 3461, 3510, 3510, 3572, 3461, 3249 },
        3461, "Cancer", new Vector3(0, 0, 0), 300, 35),

        (new int[] { 603, 226, 269, 337, 165, 15 },
        new int[] { 603, 337, 337, 269, 269, 226, 337, 165, 165, 15 },
        337, "Andromeda", new Vector3(0, 0, 0), 300, 35),

        (new int[] { 21, 168, 264, 403, 542 },
        new int[] { 21, 168, 168, 264, 264, 403, 403, 542 },
        264, "Cassiopea", new Vector3(0, 0, 0), 350, 35),

        (new int[] { 834, 915, 1017, 1122, 1220, 1228, 1203, 1131, 936, 921, 840 },
        new int[] { 834, 915, 915, 1017, 1017, 936, 936, 921, 921, 840, 1017, 1122, 1122, 1220,
            1220, 1228, 1228, 1203, 1203, 1131 },
        1122, "Perseus", new Vector3(0, 0, 0), 300, 35),

        (new int[] { 5460, 5267, 5132, 5231, 4819, 4743, 4621, 4460, 4467, 5249, 5193, 5440, 5576, 5190, 5288, 5089, 5028 },
        new int[] { 5460, 5267, 5267, 5132, 5132, 5231, 5231, 5249, 5231, 4819, 4819, 4743, 4743, 4621, 4621, 4460, 4460, 4467, 5249, 5193,
            5193, 5190, 5193, 5440, 5440, 5576, 5190, 5288, 5190, 5089, 5089, 5028 },
        5231, "Centaurus", new Vector3(0, 0, 0), 300, 35),

        (new int[] { 5511, 5264, 5107, 4910, 4932, 4825, 4689, 4517, 5056, 5315, 5338, 5487 },
        new int[] { 5511, 5264, 5264, 5107, 5107, 4910, 4910, 4932, 4910, 4825, 4825, 4689, 4689, 4517, 4825, 5056,
            5107, 5056, 5056, 5315, 5315, 5338, 5338, 5487 },
        5107, "Virgo", new Vector3(0, 0, 0), 200, 35),

        (new int[] { 5020, 4958, 4552, 4450, 4232, 4094, 3994, 3970, 3903, 3748, 3759, 3787, 3665, 3547, 3492, 3454, 3418, 3410, 3482 },
        new int[] { 5020, 4958, 4958, 4552, 4552, 4450, 4450, 4232, 4232, 4094, 4094, 3994, 3994, 3970, 3970, 3903, 3903, 3748, 3748, 3759, 3759, 3787, 3787, 3665, 3665, 3547, 3547, 3492, 3492, 3454,
            3454, 3418, 3418, 3410, 3410, 3482, 3482, 3492 },
        3970, "Hydra", new Vector3(0, 0, 0), 400, 45),

        (new int[] { 6527, 6580, 6615, 6553, 6380, 6262, 6247, 6241, 6165, 6134, 5984, 5953, 5944 },
        new int[] { 6527, 6580, 6580, 6615, 6615, 6553, 6553, 6380, 6380, 6262, 6262, 6247, 6247, 6241, 6241, 6165, 6165, 6134, 6134, 5984, 6134, 5953, 6134, 5944 },
        6241, "Scorpion", new Vector3(0, 0, 0), 400, 35),

        (new int[] { 39, 15, 8775, 8650, 8454, 8684, 8667, 8430, 8315, 8781, 8665, 8634, 8450, 8308 },
        new int[] { 39, 15, 15, 8775, 8775, 8650, 8650, 8454, 8775, 8684, 8684, 8667, 8667, 8430, 8430, 8315, 8775, 8781, 8781, 8665, 8781, 39,
            8665, 8634, 8634, 8450, 8450, 8308 },
        8781, "Pegasus", new Vector3(20, 20, 20), 400, 35),

        (new int[] { 291, 383, 360, 437, 510, 596, 549, 489, 434, 294, 224, 80, 9072, 8969, 8916, 8852, 8911, 8984 },
        new int[] { 291, 360, 291, 383, 383, 360, 360, 437, 437, 510, 510, 596, 596, 549, 549, 489, 489, 434, 434, 294, 294, 224, 224, 80, 80, 9072, 9072, 8969, 8969, 8916, 8916, 8852, 8852, 8911, 8911, 8984, 8984, 8969 },
        434, "Pisces", new Vector3(0, 0, 0), 400, 30),
        (new int[] { 1696, 1705, 1702, 1757, 1756, 1654, 1829, 1865, 1983, 2035, 1998, 2085, 2155 },
        new int[] { 1696, 1705, 1705, 1702, 1702, 1654, 1654, 1829, 1829, 1865, 1829, 1983, 1983, 2035, 2035, 1865, 1702, 1865, 1865, 1998, 1998, 2085, 2085, 2155 },
        1865, "Lepus", new Vector3(0, 0, 0), 400, 30),

        (new int[] { 2574, 2657, 2596, 2491, 2294, 2429, 2414, 2653, 2693, 2646, 2618, 2282, 2749, 2827 },
        new int[] { 2574, 2657, 2574, 2596, 2657, 2596, 2596, 2491, 2491, 2294, 2491, 2653, 2491, 2429, 2429, 2414, 2653, 2693, 2693, 2646, 2646, 2618, 2618, 2282, 2693, 2749, 2749, 2827 },
        2653, "Canis Major", new Vector3(0, 0, 0), 200, 30),

        (new int[] {4534, 4357, 4359, 3982, 3975, 4057, 3975, 4031, 3905, 3873},
        new int[] {4534, 4359, 4534, 4357, 4359, 4357, 4359, 3982, 3982, 3975, 3975, 4057, 4357, 4057, 4057, 4031, 4031, 3905, 3905, 3873},
        4357, "Leo", new Vector3(0, 0, -50), 300, 35),

        (new int[] {4100, 4247, 3974, 3800},
        new int[] {4100, 4247, 4100, 3974, 3974, 3800, 4247, 3974},
        4100, "Leo Minor", new Vector3(0, 0, 0), 150, 35),

        (new int[] {6116, 5735, 5563, 5903, 6322, 6789, 424},
        new int[] {6116, 5735, 5735, 5563, 5563, 5903, 5903, 6322, 6322, 6789, 6789, 424, 6116, 5903},
        5903, "Ursa Minor", new Vector3(0, 0, 0), 200, 30),

        (new int[] {5908, 5787, 5685, 5531, 5603},
        new int[] {5908, 5787, 5787, 5685, 5685, 5531, 5531, 5603, 5787, 5603},
        5787, "Libra", new Vector3(0, 0, 0), 300, 30),

        (new int[] {472, 566, 674, 721, 789, 794, 897, 1008, 1190, 1195, 1347, 1393, 1464, 1173, 1088, 1003, 919, 818, 874, 984, 1084, 1136, 1463, 1520, 1560, 1666, 1679, 1481},
        new int[] {472, 566, 566, 674, 674, 721, 721, 789, 789, 794, 794, 897, 897, 1008, 1008, 1190, 1190, 1195, 1195, 1347, 1347, 1393, 1393, 1464, 1464, 1173, 1173, 1088, 1088, 1003, 1003, 919, 919, 818,
        818, 874, 874, 984, 984, 1084, 1084, 1136, 1136, 1463, 1463, 1520, 1520, 1560, 1560, 1666, 1666, 1679, 1679, 1481},
        1195, "Eridanus", new Vector3(20, 20, 20), 500, 50),

        (new int[] {6705, 6536, 6554, 6688, 7310, 7582, 7352, 6927, 6396, 6132, 5986, 5744, 5291, 4787, 4434},
        new int[] {6705, 6536, 6536, 6554, 6554, 6688, 6705, 6688, 6688, 7310, 7310, 7582, 7582, 7352, 7352, 6927, 6927, 6396, 6396, 6132, 6132, 5986, 5986,5744, 5744, 5291, 5291, 4787, 4787, 4434},
        5986, "Draco", new Vector3(20, 20, 20), 400, 35),

        (new int[] {1035, 1204, 1542, 1148, 1686},
        new int[] {1035, 1148, 1148, 1686, 1148, 1542, 1542, 1204, 1035, 1204},
        1148, "Camelopardalis", new Vector3(20, 20, 20), 500, 25),

        (new int[] {8892, 8841, 8698, 8597, 8559, 8518, 8414, 8232, 7950, 8499, 8418, 8573, 8679, 8709, 8812},
        new int[] {8892, 8841, 8841, 8698, 8698, 8597, 8597, 8559, 8559, 8518, 8518, 8414, 8414, 8232, 8232, 7950, 8414, 8499, 8499, 8418, 8499, 8573, 8573, 8679, 8679, 8709, 8709, 8812},
        8499, "Aquaris", new Vector3(0, 0, 0), 400, 35),

        (new int[] {8720, 8576, 8447, 8326, 8326, 8386, 8628, 8728},
        new int[] {8720, 8576, 8576, 8447, 8447, 8326, 8326, 8386, 8386, 8628, 8628, 8728},
        8576, "Piscis Austrinus",new Vector3(20, 20, 0), 500, 25),

        (new int[] {280 ,105, 8937, 8863},
        new int[] {280 ,105, 105, 8937, 8937, 8863, 8863, 280},
        105, "Sculptor", new Vector3(10, 10, 10), 400, 30),

        (new int[] {7602, 7557, 7525, 7377, 7570, 7710, 7235, 7176, 7236},
        new int[] {7602, 7557, 7557, 7525, 7557, 7377, 7377, 7235, 7235, 7176, 7377, 7570, 7570, 7710, 7377, 7236},
        7377 , "Aquila", new Vector3(10, 10, 10), 400, 30),

        (new int[] {896, 813, 718, 649, 754, 804, 911, 779, 681, 781, 811, 740, 509, 188, 74, 334, 402, 334, 402, 539, 708},
        new int[] {911, 896, 896, 813, 813, 718, 718, 649, 718, 754, 754, 804, 911, 804, 804, 779, 779, 681, 681, 781, 781, 811, 811, 740, 740, 509, 509, 188, 188, 74, 188, 334, 334, 402, 402, 539, 539, 708, 708, 781},
        681, "Cetus", new Vector3(10, 10, 10), 400, 35),

        (new int[] {6897, 6905},
        new int[] {6897, 6905},
        6897, "Telescopium", new Vector3(10, 10, 10), 300, 20),

        (new int[] {7063, 6973, 7063, 7119, 6930},
        new int[] {7063, 7066, 7066, 7119, 7119, 6930, 6930, 6973, 6973, 7063},
        7066, "Scutum", new Vector3(10, 10, 10), 400, 30),

        (new int[] {7948, 7906, 7882, 7928, 7852},
        new int[] {7948, 7906, 7906, 7882, 7882, 7928, 7948, 7928, 7882, 7852},
        7882, "Delphinus", new Vector3(10, 10, 10), 400, 30),

        (new int[] {6582, 6745, 6855, 7074, 7107, 6982, 7590, 7665, 7913, 8181, 7790},
        new int[] {6582, 6745, 6745, 6855, 6855, 7074, 6745, 7074, 7074, 7107, 7107, 6982, 6982, 7590, 7590, 7665, 7107, 7665, 7665, 7913, 7913, 8181, 8181, 7790, 7665, 7790},
        7107, "Pavo", new Vector3(10, 10, 10), 400, 35),

        (new int[] {6510, 6285, 6229, 6500, 6462, 6461, 6743},
        new int[] {6510, 6285, 6285, 6229, 6229, 6500, 6500, 6462, 6462, 6461, 6461, 6743, 6743, 6510},
        6461, "Ara", new Vector3(10, 10, 10), 400, 30),

        (new int[] {5470, 6102, 6163},
        new int[] {5470, 6102, 6102, 6163},
        6102, "Apus", new Vector3(10, 10, 10), 300, 25),

        (new int[] {5962, 6115, 6072, 6024},
        new int[] {5962, 6115, 6115, 6072, 6072, 5962, 6072, 6024, 6024, 5962},
        6072, "Norma", new Vector3(10, 10, 10), 400, 30),

        (new int[] {4520, 4844, 4798, 4773},
        new int[] {4520, 4844, 4844, 4798, 4798, 4773, 4773, 4520},
        4798, "Musca", new Vector3(10, 10, 10), 400, 30),

        (new int[] {4757, 4662, 4630, 4623, 4786},
        new int[] {4757, 4662, 4662, 4630, 4630, 4623, 4630, 4786, 4757, 4662},
        4630, "Corvus", new Vector3(10, 10, 10), 400, 30),

        (new int[] {4468, 4382, 4287, 4343, 4405, 4514, 4567},
        new int[] {4468, 4382, 4382, 4287, 4382, 4405, 4287, 4343, 4343, 4405, 4405, 4514, 4514, 4567, 4567, 4468},
        4405, "Crater", new Vector3(10, 10, 10), 400, 30),

        (new int[] {8140, 7986, 7869},
        new int[] {8140, 7986, 7986, 7869, 7869, 8140},
        8140, "Indi", new Vector3(10, 10, 10), 300, 25),

        (new int[] {8820, 8747, 8636, 8675, 8425, 8787, 8556, 5411, 8353},
        new int[] {8820, 8636, 8636, 8747, 8636, 8675, 8636, 8425, 8425, 8556, 8556, 8787, 8787, 8820, 8425, 8411, 8411, 8353},
        8636, "Grus", new Vector3(10, 10, 10), 400, 30),

        (new int[] {5602, 5681, 5506, 5340, 5477, 5235, 5200, 5429, 5435},
        new int[] {5602, 5681, 5681, 5506, 5506, 5340, 5340, 5477, 5340, 5235, 5235, 5200, 5340, 5429, 5429, 5435, 5435, 5602},
        5340, "Bootes", new Vector3(10, 10, 10), 400, 30),

        (new int[] {8309, 8115, 7949, 7796, 7924, 7615, 7417, 2528, 7420, 7328},
        new int[] {8309, 8115, 8115, 7949, 7949, 7796, 7796, 7924, 7796, 7528, 7528, 7420, 7420, 7328, 7796, 7615, 7615, 7417},
        7796, "Cygnus", new Vector3(10, 10, 10), 400, 30),

        (new int[] {838, 617, 553, 545},
        new int[] {838, 617, 617, 553, 553, 545},
        617, "Aries", new Vector3(10, 10, 10), 400, 30),

        (new int[] {555, 440, 322, 429, 338, 100, 498, 99},
        new int[] {555, 440, 555, 322, 440, 322, 322, 429, 322, 338, 322, 100, 429, 100, 338, 100, 100, 99, 100, 498, 99, 498},
        322, "Phoenix", new Vector3(10, 10, 10), 400, 35),

        (new int[] {7141, 6869, 6561, 6378, 6175, 6075, 6056, 5892, 5854, 5789, 5867, 5933, 5879},
        new int[] {7141, 6869, 6869, 6581, 6581, 6561, 6561, 6378, 6378, 6175, 6175, 6075, 6075, 6056, 6056, 5892, 5892, 5854, 5854, 5789, 5789, 5867, 5867, 5879, 5879, 5933, 5867, 5933},
        6056, "Serpens", new Vector3(10, 10, 10), 400, 35),

        (new int[] {6519, 6378, 6603, 6556, 6299, 6056},
        new int[] {6603, 6556, 6556, 6299, 6299, 6056, 6603, 6378, 6378, 6519},
        6603, "Ophiuchus", new Vector3(10, 10, 10), 400, 35),

        (new int[] {2238, 2560, 2818, 3275, 3275, 3579, 3612, 3690, 3705},
        new int[] {2238, 2560, 2560, 2818, 2818, 3275, 3275, 3579, 3579, 3612, 3612, 3690, 3690, 3705},
        3275, "Lynx", new Vector3(10, 10, 10), 400, 30),

        (new int[] {6588, 6695, 6485, 6418, 6324, 6410, 6526, 6623, 6703, 6779, 6220, 6168, 6092, 5914, 6212, 6148, 6095, 6406},
        new int[] {6588, 6695, 6695, 6485, 6485, 6418, 6418, 6324, 6324, 3410, 3410, 6526, 6526, 6623, 6623, 6703, 6703, 6418,6418,  6220, 6220, 6168, 6168, 6692, 6692, 5914, 6324, 6212, 6220, 6212, 5212, 6148,
        6148, 6406, 6148, 6095},
        6323, "Hercules", new Vector3(10, 10, 10), 400, 35),

        (new int[] {8974, 8694, 8465, 8162, 8238},
        new int[] {8974, 8694, 8694, 8465, 8465, 8162, 8152, 8238, 8238, 8694, 8238, 8974},
        8694, "Cepheus", new Vector3(20, 20, 20), 400, 30),


    };
    
    void Update()
    {
        Vector2 cursorPosition = Input.mousePosition;

        for (int i = 0; i < constellations.Count; i++)
        {
            GameObject mainStar = starObjects[constellations[i].Item3 - 1];
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(mainStar.transform.position);

            float distance = Vector2.Distance(new Vector2(screenPosition.x, screenPosition.y), cursorPosition);

            if (distance < 100f)  
            {
                ToggleConstellation(i, true);
                //canvas.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

            }
            else
            {
                ToggleConstellation(i, false);  
            }
        }
    }

    void ToggleConstellation(int index, bool show)
    {
        if ((index < 0) || (index >= constellations.Count))
            return;

        if (show && !constellationVisible.ContainsKey(index))
        {
            CreateConstellation(index);
        }
        else if (!show && constellationVisible.ContainsKey(index))
        {
            RemoveConstellation(index);
        }
    }

    private IEnumerator AnimateLine (Vector3 startPos, Vector3 endPos, LineRenderer lr)
    {
        float startTime = Time.time;

        Vector3 pos = startPos;
        Vector3 dir = (endPos - startPos).normalized * 4;
        lr.positionCount = 2;
        lr.SetPosition(0, pos + dir);
        while ((Time.time - startTime < animationDuration) && lr != null)
        {
            float t = (Time.time - startTime) / animationDuration;
            pos = Vector3.Lerp(startPos, endPos, t);
            lr.SetPosition(1, pos - dir);
            yield return null;
        }
        //lr.SetPosition(1, endPos);
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        float currentAlpha = i.color.a;
        float targetAlpha = 0.2f;

        while (currentAlpha < targetAlpha && i != null)
        {
            currentAlpha = Mathf.Min(currentAlpha + (Time.deltaTime / t), targetAlpha);
            i.color = new Color(i.color.r, i.color.g, i.color.b, currentAlpha);
            // Optional efficiency check
            if (currentAlpha >= targetAlpha)
            {
                break;
            }

            yield return null;
        }
    }

    string SpacesbetweenCharacters(string s)
    {
        string result = "";
        for (byte i = 0; i<s.Length; i++)
        {
            result = result + s[i] + " "; 
        }
        return result;
    }


    void CreateConstellation(int index)
    {
        Material lineMaterial = Resources.Load("LineMaterial", typeof(Material)) as Material;
        int[] constellation = constellations[index].Item1;
        int[] lines = constellations[index].Item2;
        int mainStarIndex = constellations[index].Item3;
        string constellationName = constellations[index].Item4;
        Vector3 textOffset = constellations[index].Item5;
        int canvasWitdth = constellations[index].Item6;
        int fontSize = constellations[index].Item7;


        Debug.Log(constellationName);
        // změnit barvu hvězd na bílou
        foreach (int catalogNumber in constellation)
        {
            
            starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = Color.white;
        }
       
        GameObject constellationHolder = new($"Constellation {index}");
        constellationHolder.transform.parent = transform;
        constellationVisible[index] = constellationHolder;

        // vykreslení souhvězdí
        for (int i = 0; i < lines.Length; i += 2)
        {
            // Parent k tomu aby jsme souhvězdí mohli vymazat
            GameObject line = new("Line");
            line.transform.parent = constellationHolder.transform;
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

            //lineMaterial.shader = Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply");
            lineRenderer.material = lineMaterial;
            lineRenderer.startWidth = 3f;
            lineRenderer.endWidth = 3f;
            lineRenderer.useWorldSpace = false;
            Vector3 pos1 = starObjects[lines[i] - 1].transform.position;
            Vector3 pos2 = starObjects[lines[i + 1] - 1].transform.position;

            StartCoroutine(AnimateLine(pos1, pos2, lineRenderer));


        }

        Vector3 mainStarPos = starObjects[mainStarIndex - 1].transform.position;

        Canvas canvas = new GameObject("Constellation Canvas" + index.ToString()).AddComponent<Canvas>();
        canvas.transform.SetParent(constellationHolder.transform);

        canvas.renderMode = RenderMode.WorldSpace;
        canvas.transform.position = mainStarPos;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasWitdth, canvas.GetComponent<RectTransform>().sizeDelta.y);
        canvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        Text textObject = new GameObject("Constellation Text").AddComponent<Text>();
        textObject.transform.SetParent(canvas.transform);

       
        textObject.transform.position = mainStarPos + textOffset;
        textObject.text = SpacesbetweenCharacters(constellationName.ToUpper());
        textObject.color = new Color(0.6f, 0.83f, 0.95f, 0f);
        //textObject.color = new Color( 0.6f, 0.83f, 0.95f, 0.5f);
        Font textFont = Resources.Load<Font>("PerfectDOSVGA437");
        textObject.font = textFont; 
        textObject.fontSize = fontSize;
        //textObject.material = new Material(Resources.Load("lineMaterial", typeof(Material)) as Material);
        //textObject.material.color = new Color(0.6f, 0.83f, 0.95f, 0.5f);
        textObject.rectTransform.sizeDelta = new Vector2(canvasWitdth, textObject.rectTransform.sizeDelta.y);
        textObject.alignment = TextAnchor.MiddleCenter;
        textObject.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

        StartCoroutine(FadeTextToFullAlpha(2f, textObject));
        
        
            
    }

    void RemoveConstellation(int index)
    {
        int[] constellation = constellations[index].Item1;

        foreach (int catalogNumber in constellation)
        {
            // Remember list is 0-up catalog numbers are 1-up.
            starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = stars[catalogNumber - 1].color;
        }
        // Remove the constellation lines.
        Destroy(constellationVisible[index]);
        // Remove from our dictionary as it's now off.
        constellationVisible.Remove(index);
    }

}

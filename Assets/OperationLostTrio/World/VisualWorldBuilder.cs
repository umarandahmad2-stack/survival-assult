using System.Collections.Generic;
using UnityEngine;

namespace OperationLostTrio
{
    /// <summary>
    /// Quick blockout builder for a readable, textured urban combat map.
    /// Add to an empty GameObject and press Play, or use the context menu.
    /// Replace primitive geometry with authored models as the project grows.
    /// </summary>
    public class VisualWorldBuilder : MonoBehaviour
    {
        public bool buildOnStart = true;
        public int randomSeed = 44;
        [Range(2, 8)] public int buildingRows = 4;
        public float blockSpacing = 22f;
        public bool createLighting = true;

        readonly List<GameObject> spawned = new List<GameObject>();
        Material asphalt, concrete, brick, roof, glass, metal, hazard, grass;
        Transform worldRoot;

        void Start() { if (buildOnStart) BuildWorld(); }

        [ContextMenu("Build Textured Survival Map")]
        public void BuildWorld()
        {
            ClearWorld();
            Random.InitState(randomSeed);
            CreateMaterials();
            worldRoot = new GameObject("Generated Survival District").transform;
            worldRoot.SetParent(transform, false);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = new Color(0.43f, 0.49f, 0.52f);
            RenderSettings.fogDensity = 0.0045f;
            RenderSettings.ambientLight = new Color(0.48f, 0.51f, 0.54f);

            MakeCube("Terrain", new Vector3(0, -0.7f, 0), new Vector3(150, 1, 150), grass);
            MakeCube("Main road", new Vector3(0, -0.12f, 0), new Vector3(14, 0.12f, 150), asphalt);
            for (int i = -6; i <= 6; i++)
            {
                if (i % 2 == 0) MakeCube("Road dash", new Vector3(0, -0.045f, i * 10), new Vector3(0.22f, 0.025f, 4.5f), hazard);
            }
            MakeCube("West pavement", new Vector3(-9, -0.08f, 0), new Vector3(4, 0.18f, 150), concrete);
            MakeCube("East pavement", new Vector3(9, -0.08f, 0), new Vector3(4, 0.18f, 150), concrete);

            for (int row = 0; row < buildingRows; row++)
            {
                float z = (row - (buildingRows - 1) / 2f) * blockSpacing;
                BuildBuilding("West block " + row, -25f, z, Random.Range(10f, 17f), Random.Range(9f, 20f), Random.Range(9f, 15f));
                BuildBuilding("East block " + row, 25f, z, Random.Range(10f, 17f), Random.Range(9f, 20f), Random.Range(9f, 15f));
                BuildCover(new Vector3(-14, 0, z + 6));
                BuildCover(new Vector3(14, 0, z - 6));
            }

            // Ruined checkpoint and a few recognizable points of interest.
            MakeCube("Checkpoint barrier", new Vector3(0, 0.65f, -28), new Vector3(11, 1.3f, 1.2f), hazard);
            MakeCube("Checkpoint booth", new Vector3(5.8f, 1.5f, -28), new Vector3(3.2f, 3f, 3.2f), metal);
            MakeCube("Supply container", new Vector3(-5.5f, 1.4f, 22), new Vector3(4.2f, 2.8f, 7f), metal);
            MakeCube("Container door", new Vector3(-5.5f, 1.2f, 18.4f), new Vector3(2.4f, 2.2f, 0.12f), hazard);

            for (int i = 0; i < 24; i++)
            {
                float x = Random.Range(-55f, 55f);
                float z = Random.Range(-65f, 65f);
                if (Mathf.Abs(x) < 12f || Mathf.Abs(x) > 48f) continue;
                MakeCube("Rubble", new Vector3(x, Random.Range(0.15f, 0.5f), z),
                    new Vector3(Random.Range(0.4f, 1.4f), Random.Range(0.3f, 1f), Random.Range(0.4f, 1.4f)),
                    Random.value > 0.5f ? concrete : brick).transform.rotation = Quaternion.Euler(Random.Range(-15,15), Random.Range(0,180), Random.Range(-15,15));
            }

            if (createLighting) CreateSun();
        }

        void BuildBuilding(string label, float x, float z, float width, float height, float depth)
        {
            Material wall = Random.value > 0.5f ? concrete : brick;
            MakeCube(label + " shell", new Vector3(x, height / 2f, z), new Vector3(width, height, depth), wall);
            MakeCube(label + " roof", new Vector3(x, height + 0.12f, z), new Vector3(width + 0.4f, 0.24f, depth + 0.4f), roof);

            // Recess-like dark window panels and bright trim make the facades readable.
            int floors = Mathf.Max(1, Mathf.FloorToInt(height / 3.2f));
            int windows = Mathf.Max(2, Mathf.FloorToInt(width / 2.5f));
            for (int floor = 0; floor < floors; floor++)
            for (int w = 0; w < windows; w++)
            {
                float wx = x - width * 0.38f + w * (width * 0.76f / Mathf.Max(1, windows - 1));
                float wy = 1.8f + floor * 3.1f;
                MakeCube(label + " window", new Vector3(wx, wy, z - depth / 2f - 0.035f), new Vector3(0.85f, 1.15f, 0.08f), glass);
                MakeCube(label + " sill", new Vector3(wx, wy - 0.63f, z - depth / 2f - 0.11f), new Vector3(1.02f, 0.1f, 0.18f), metal);
            }
            MakeCube(label + " entrance", new Vector3(x, 1.15f, z - depth / 2f - 0.06f), new Vector3(1.4f, 2.3f, 0.12f), metal);
            MakeCube(label + " sign", new Vector3(x, height + 0.8f, z - depth / 2f - 0.08f), new Vector3(Mathf.Min(width * 0.7f, 5f), 0.45f, 0.16f), hazard);
        }

        void BuildCover(Vector3 p)
        {
            MakeCube("Concrete cover", p + Vector3.up * 0.65f, new Vector3(2.8f, 1.3f, 1.2f), concrete);
            MakeCube("Cover warning stripe", p + Vector3.up * 1.31f, new Vector3(2.8f, 0.08f, 1.22f), hazard);
        }

        void CreateMaterials()
        {
            asphalt = MakeMaterial("Road | gritty charcoal", new Color(0.16f,0.18f,0.19f), 0.05f, 0.1f, 0.8f);
            concrete = MakeMaterial("Concrete | weathered", new Color(0.52f,0.53f,0.49f), 0f, 0.2f, 0.6f);
            brick = MakeMaterial("Brick | dusty red", new Color(0.43f,0.22f,0.17f), 0f, 0.1f, 0.65f);
            roof = MakeMaterial("Roof | dark slate", new Color(0.19f,0.22f,0.23f), 0f, 0.1f, 0.6f);
            glass = MakeMaterial("Window | blue-black glass", new Color(0.055f,0.16f,0.2f), 0.55f, 0.7f, 0.25f);
            metal = MakeMaterial("Metal | scratched olive", new Color(0.25f,0.29f,0.24f), 0.45f, 0.35f, 0.45f);
            hazard = MakeMaterial("Hazard | rescue orange", new Color(0.95f,0.36f,0.06f), 0f, 0.1f, 0.45f);
            grass = MakeMaterial("Ground | dry field", new Color(0.29f,0.34f,0.25f), 0f, 0.0f, 0.95f);
            asphalt.mainTexture = MakeNoiseTexture(new Color(0.7f,0.7f,0.7f), 128, 17);
            concrete.mainTexture = MakeNoiseTexture(new Color(0.9f,0.9f,0.9f), 128, 31);
            brick.mainTexture = MakeBrickTexture(128, 24);
            grass.mainTexture = MakeNoiseTexture(new Color(0.75f,0.8f,0.7f), 128, 55);
            asphalt.mainTextureScale = new Vector2(8, 8);
            concrete.mainTextureScale = new Vector2(2, 2);
            brick.mainTextureScale = new Vector2(2, 2);
            grass.mainTextureScale = new Vector2(10, 10);
        }

        Material MakeMaterial(string label, Color color, float metallic, float smoothness, float noiseStrength)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            Material mat = new Material(shader) { name = label, color = color };
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
            return mat;
        }

        Texture2D MakeNoiseTexture(Color tint, int size, int seed)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGB24, true) { name = "Generated grunge texture", wrapMode = TextureWrapMode.Repeat };
            Random.State old = Random.state; Random.InitState(seed);
            for (int y=0; y<size; y++) for (int x=0; x<size; x++) {
                float n = Random.Range(0.72f, 1.08f);
                if (Random.value > 0.995f) n *= 0.45f;
                tex.SetPixel(x,y,new Color(tint.r*n,tint.g*n,tint.b*n));
            }
            tex.Apply(true); Random.state = old; return tex;
        }

        Texture2D MakeBrickTexture(int size, int rows)
        {
            Texture2D tex = new Texture2D(size,size,TextureFormat.RGB24,true) { name="Generated brick texture", wrapMode=TextureWrapMode.Repeat };
            for (int y=0; y<size; y++) for (int x=0; x<size; x++) {
                bool mortar = y % (size/rows) < 3 || (x + ((y/(size/rows))%2)*(size/2)) % (size/2) < 3;
                float n = Random.Range(0.84f,1.08f);
                tex.SetPixel(x,y,mortar ? new Color(0.55f,0.52f,0.47f) : new Color(0.58f*n,0.28f*n,0.21f*n));
            }
            tex.Apply(true); return tex;
        }

        GameObject MakeCube(string label, Vector3 position, Vector3 scale, Material mat)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = label; obj.transform.SetParent(worldRoot, false); obj.transform.position = position; obj.transform.localScale = scale;
            obj.GetComponent<Renderer>().sharedMaterial = mat; spawned.Add(obj); return obj;
        }

        void CreateSun()
        {
            GameObject sun = new GameObject("Generated cinematic sun");
            sun.transform.SetParent(worldRoot, false); sun.transform.rotation = Quaternion.Euler(38f, -32f, 0f);
            Light light = sun.AddComponent<Light>(); light.type = LightType.Directional; light.intensity = 1.15f;
            light.color = new Color(1f,0.86f,0.69f); light.shadows = LightShadows.Soft;
        }

        void ClearWorld()
        {
            Transform old = transform.Find("Generated Survival District");
            if (old != null) { if (Application.isPlaying) Destroy(old.gameObject); else DestroyImmediate(old.gameObject); }
            spawned.Clear();
        }
    }
}
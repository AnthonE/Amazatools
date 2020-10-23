using UnityEngine;

namespace Amazatools.Compression
{    /// <summary>
     /// Place on a gameobject, fill out the test float, and then place two preview objects to test.
     /// The two top lines inside UpdateTest are the meat.
     /// </summary>
    public class TestFloatCompression : MonoBehaviour
    {
         
        public float TestFloat = 1234.567f;
        public Transform PreviewUncompressed;
        public Transform PreviewCubeCompressed;
        [Header("Real time status:")]
        [SerializeField] private float TestFloatAfterDecompression;
        [SerializeField] private float CompressionDiffrence;
        [SerializeField] private int CompressedBits;
        public FloatCompressionSettings TestCompressionSettings = new FloatCompressionSettings { Accuracy = 0.02f, Min = -5000, Max = 5000 };
   
        void Start()
        {
            Debug.Log("Starting Compression Test");
            InvokeRepeating(nameof(UpdateTest), 1f, 1f);
            Camera.main.transform.position= new Vector3(TestFloat, 0, -3);
            PreviewUncompressed.gameObject.GetComponent<Renderer>().material.color = Color.red;
            PreviewCubeCompressed.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        void UpdateTest()
        {
            FloatCompression FloatCompression = FloatCompressor.TotalCompression(TestFloat, TestCompressionSettings);
            TestFloatAfterDecompression = FloatCompressor.TotalUnCompression(FloatCompression);

            if (TestCompressionSettings.Min < TestFloatAfterDecompression && TestFloatAfterDecompression < TestCompressionSettings.Max)
            {
                CompressionDiffrence = TestFloat - TestFloatAfterDecompression;
                CompressedBits = FloatCompression._bits;
                Debug.Log("Uncompressed float was:" + TestFloat + " | Compressed uint: " + FloatCompression._compressed + " | Decompressed float: " + TestFloatAfterDecompression);
                Debug.Log("This was a difference of:" + CompressionDiffrence + " | This is a total bit size of:" + CompressedBits);
                PreviewUncompressed.position = new Vector3(TestFloat, 0,0);
                PreviewCubeCompressed.position = new Vector3(TestFloatAfterDecompression, 0, 0);
            }
            else
            {
                if (TestCompressionSettings.Min > TestFloatAfterDecompression)
                {
                    Debug.LogWarning("The TestFloat is lower then TestCompressionSettings.Min here is the result:" + TestFloatAfterDecompression);
                }
                else
                {
                    Debug.LogWarning("The TestFloat is higher then TestCompressionSettings.mac here is the result:" + TestFloatAfterDecompression);
                }
            }
        }
    }
}

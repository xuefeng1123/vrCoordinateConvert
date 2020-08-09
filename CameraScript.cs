using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TakeVideo();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    VideoCapture m_VideoCapture = null;
    bool isRecording;
    public void StopVideo()
    {
        if (isRecording)
        {
            isRecording = false;
            print("停止录像...");
            if (Application.platform == RuntimePlatform.WSAPlayerX86)
                m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
        }
    }
    public void TakeVideo()
    {
        //VideoImage.gameObject.SetActive(false);
        //ShowImage.gameObject.SetActive(false);

        if (!isRecording)
        {

            isRecording = true;
            print("开始录像...");
            if (Application.platform == RuntimePlatform.WSAPlayerX86)
                VideoCapture.CreateAsync(false, StartVideoCapture);
        }
    }

    void StartVideoCapture(VideoCapture videoCapture)
    {

        if (videoCapture != null)
        {
            m_VideoCapture = videoCapture;
            Debug.Log("Created VideoCapture Instance!");

            Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();
            Debug.Log("刷新率：" + cameraFramerate);

            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.frameRate = cameraFramerate;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            m_VideoCapture.StartVideoModeAsync(cameraParameters,
                VideoCapture.AudioState.ApplicationAndMicAudio,
                OnStartedVideoCaptureMode);
        }
        else
        {
            Debug.LogError("Failed to create VideoCapture Instance!");
        }
    }

    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("开始录像模式!");
        //string timeStamp = Time.time.ToString().Replace(".", "").Replace(":", "");
        //string filename = string.Format("TestVideo_{0}.mp4", timeStamp);
        string filename = "TestVideo.mp4";
        string filepath = Path.Combine(Application.persistentDataPath, filename);
        filepath = filepath.Replace("/", @"\");
        m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
        print("videopath:" + filepath);
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        m_VideoCapture.Dispose();
        m_VideoCapture = null;
        Debug.Log("停止录像模式!");
    }

    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("开始录像!");
    }

    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("停止录像!");
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }
}

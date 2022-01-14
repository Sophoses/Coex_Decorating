using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maxstAR;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class MaxstSceneManager : MonoBehaviour
{
	private CameraBackgroundBehaviour cameraBackgroundBehaviour = null;
	private GameObject arCamera = null;
	private VPSStudioController vPSStudioController = null;

	public List<GameObject> disableObjects = new List<GameObject>();
	public List<GameObject> occlusionObjects = new List<GameObject>();
	public List<GameObject> Stickers = new List<GameObject>();
	private List<VPSTrackable> vPSTrackablesList = new List<VPSTrackable>();

	public RaycastHit vHit;
	public RectTransform btn1;
	public GameObject btn1_go;

	public Material buildingMaterial;
	public Material runtimeBuildingMaterial;

	public GameObject maxstLogObject;
	public GameObject StickerPanel;
	public GameObject PalettePanel;
	public GameObject CustomPanel;
	public GameObject button;
	public GameObject button2;
	public GameObject backButton;
	public GameObject graffiButton;
	public GameObject plus;
	public GameObject graffi;
	public Text text;
	bool isOpacity;
	bool isPanel;
	bool isStick = false;
	bool isPlus = false;
	GameObject custom;

	public bool isOcclusion = true;
	private string currentLocalizerLocation = "";

	private string serverName = "";

	public GameObject arContent;

	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.CAMERA", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION");
		if (result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted)
			Debug.Log("We have all the permissions!");
		else
			Debug.Log("Some permission(s) are not granted...");

		ARManager arManagr = FindObjectOfType<ARManager>();
		if (arManagr == null)
		{
			Debug.LogError("Can't find ARManager. You need to add ARManager prefab in scene.");
			return;
		}
		else
		{
			arCamera = arManagr.gameObject;
		}
		
		vPSStudioController = FindObjectOfType<VPSStudioController>();
		if (vPSStudioController == null)
		{
			Debug.LogError("Can't find VPSStudioController. You need to add VPSStudio prefab in scene.");
			return;
		}
		else
        {
			string tempServerName = vPSStudioController.vpsServerName;
			serverName = tempServerName;
			vPSStudioController.gameObject.SetActive(false);
		}

		cameraBackgroundBehaviour = FindObjectOfType<CameraBackgroundBehaviour>();
		if (cameraBackgroundBehaviour == null)
		{
			Debug.LogError("Can't find CameraBackgroundBehaviour.");
			return;
		}

		VPSTrackable[] vPSTrackables = FindObjectsOfType<VPSTrackable>(true);
		if (vPSTrackables != null)
		{
			vPSTrackablesList.AddRange(vPSTrackables);
		}
		else
		{
			Debug.LogError("You need to add VPSTrackables.");
		}

		foreach (GameObject eachObject in disableObjects)
		{
			if(eachObject != null)
            {
				eachObject.SetActive(false);
			}
		}
	}

	void Start()
	{
		isOpacity = true;
		isPanel = true;
		buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 0.01f);
		//시작 시 buildingmaterial을 투명한 상태에서 시작.
		if (isOcclusion)
		{
			foreach (GameObject eachGameObject in occlusionObjects)
			{
				if (eachGameObject == null)
				{
					continue;
				}

				Renderer[] cullingRenderer = eachGameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer eachRenderer in cullingRenderer)
				{
					eachRenderer.material.renderQueue = 1900;
					eachRenderer.material = runtimeBuildingMaterial;
				}
			}
		}

		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
			string simulatePath = vPSStudioController.vpsSimulatePath;
			if (Directory.Exists(simulatePath))
			{
				CameraDevice.GetInstance().Start(simulatePath);
				MaxstAR.SetScreenOrientation((int)ScreenOrientation.Portrait);
			}
		}
		else
        {
			if (CameraDevice.GetInstance().IsFusionSupported(CameraDevice.FusionType.ARCamera))
			{
				CameraDevice.GetInstance().Start();
			}
			else
			{
				TrackerManager.GetInstance().RequestARCoreApk();
			}
		}
		
		TrackerManager.GetInstance().StartTracker();
    }

	void Update()
	{
		TrackerManager.GetInstance().UpdateFrame();

		ARFrame arFrame = TrackerManager.GetInstance().GetARFrame();

		TrackedImage trackedImage = arFrame.GetTrackedImage();

		if (trackedImage.IsTextureId())
		{
			IntPtr[] cameraTextureIds = trackedImage.GetTextureIds();
			cameraBackgroundBehaviour.UpdateCameraBackgroundImage(cameraTextureIds);
		}
		else
		{
			cameraBackgroundBehaviour.UpdateCameraBackgroundImage(trackedImage);
		}

		if (arFrame.GetARLocationRecognitionState() == ARLocationRecognitionState.ARLocationRecognitionStateNormal)
		{
			Matrix4x4 targetPose = arFrame.GetTransform();

			arCamera.transform.position = MatrixUtils.PositionFromMatrix(targetPose);
			arCamera.transform.rotation = MatrixUtils.QuaternionFromMatrix(targetPose);
			arCamera.transform.localScale = MatrixUtils.ScaleFromMatrix(targetPose);

			string localizerLocation = arFrame.GetARLocalizerLocation();

			Debug.Log(localizerLocation);

			if (currentLocalizerLocation != localizerLocation)
			{
				currentLocalizerLocation = localizerLocation;
				foreach (VPSTrackable eachTrackable in vPSTrackablesList)
				{
					bool isLocationInclude = false;
					foreach (string eachLocation in eachTrackable.localizerLocation)
					{
						if (currentLocalizerLocation == eachLocation)
						{
							isLocationInclude = true;
							break;
						}
					}
					eachTrackable.gameObject.SetActive(isLocationInclude);
				}
			}
		}
		else
		{
			foreach (VPSTrackable eachTrackable in vPSTrackablesList)
			{
				eachTrackable.gameObject.SetActive(false);
			}
			currentLocalizerLocation = "";
		}
	}

	void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			CameraDevice.GetInstance().Stop();
			TrackerManager.GetInstance().StopTracker();
		}
		else
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
			{
				string simulatePath = vPSStudioController.vpsSimulatePath;
				if (Directory.Exists(simulatePath))
				{
					CameraDevice.GetInstance().Start(simulatePath);
					MaxstAR.SetScreenOrientation((int)ScreenOrientation.Portrait);
				}
			}
			else
			{
				if (CameraDevice.GetInstance().IsFusionSupported(CameraDevice.FusionType.ARCamera))
				{
					CameraDevice.GetInstance().Start();
				}
				else
				{
					TrackerManager.GetInstance().RequestARCoreApk();
				}
			}

			TrackerManager.GetInstance().StartTracker();
		}
	}

	void OnDestroy()
	{
		CameraDevice.GetInstance().Stop();
		TrackerManager.GetInstance().StopTracker();
		TrackerManager.GetInstance().DestroyTracker();
	}

	public void OnClickNavigation()
    {
		string navigationLocation = "";
		if (currentLocalizerLocation != null)
		{
			GameObject trackingObject = null;
			foreach (VPSTrackable eachTrackable in vPSTrackablesList)
			{
				foreach (string eachLocation in eachTrackable.localizerLocation)
				{
					if (currentLocalizerLocation == eachLocation)
					{
						navigationLocation = eachTrackable.navigationLocation;
						trackingObject = eachTrackable.gameObject;
						break;
					}
				}
			}

			if (trackingObject != null)
			{
				NavigationController navigationController = GetComponent<NavigationController>();

				navigationController.MakePath(navigationLocation, arCamera.transform.position, "landmark_centralcity_f1", new Vector3(-48.353495f, 2.447647f, -53.145505f), arContent,
				() => {
					Debug.Log("No Path");
				});
			}
		}
	}
	public void OnClickPlusButton()
    {
		if (isPlus == true)
        {
			button.gameObject.SetActive(false);
			button2.gameObject.SetActive(false);
			isPlus = false;
		}
		else
        {
			button.gameObject.SetActive(true);
			button2.gameObject.SetActive(true);
			isPlus = true;
		}
	}
	public void OnClickButton()//스티커 붙이기 버튼 클릭 시 
    {
		StickerPanel.gameObject.SetActive(true);
		button.gameObject.SetActive(false);
		button2.gameObject.SetActive(false);
		plus.gameObject.SetActive(false);
		graffiButton.gameObject.SetActive(false);
		buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 0.5f);
		text.text = "스티커 꾸미기";
	}
	public void OnClickButton2()//그래피티 버튼 클릭 시 
	{
		text.text = "그래피티";
		button.gameObject.SetActive(false);
		button2.gameObject.SetActive(false);
		plus.gameObject.SetActive(false);
		graffi.gameObject.SetActive(true);
		PalettePanel.gameObject.SetActive(true);
		backButton.gameObject.SetActive(true);
	}
	public void OnClickButton3()//커스텀 버튼 클릭 시
    {
		button.gameObject.SetActive(false);
		button2.gameObject.SetActive(false);
		plus.gameObject.SetActive(false);
		buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 0.5f);
		text.text = "커스텀 만들기";
		CustomPanel.gameObject.SetActive(true);
	}
    public void OnClickBackButton()//완료버튼 클릭 시
    {
		buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 0.01f);
		text.text = "나만의 코엑스를 만들어보세요";
		StickerPanel.gameObject.SetActive(false);
		PalettePanel.gameObject.SetActive(false);
		plus.gameObject.SetActive(true);
		graffiButton.gameObject.SetActive(true);
		graffi.gameObject.SetActive(false);
		backButton.gameObject.SetActive(false);
		btn1_go.gameObject.SetActive(false);
		isStick = false;
	}

	public void Done()
    {
		custom = GameObject.Find("Customs");
		isStick = true;
		maxstLogObject = custom;
		CustomPanel.gameObject.SetActive(false);
		backButton.gameObject.SetActive(true);
	}

	public void ChooseSticker()
    {
		GameObject clickObject = EventSystem.current.currentSelectedGameObject;
		if (clickObject.name.Substring(0,3) == "stk")
        {
			isStick = true;
			int index = int.Parse(clickObject.name.Substring(3, 1)) - 1;
			maxstLogObject = Stickers[index];
			StickerPanel.gameObject.SetActive(false);
			backButton.gameObject.SetActive(true);
		}
    }
	void FixedUpdate()
	{
		if(Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
			Vector2 vTouchPos = Input.mousePosition;

			Ray ray = Camera.main.ScreenPointToRay(vTouchPos);

			if (Physics.Raycast(ray.origin, ray.direction, out vHit))
            {
				if (isStick == true)
					AttachLogo(vHit);
                if (isStick == false && vHit.collider.name.Substring(0, 3) == "skr")
                {
					Vector2 mousePos = Input.mousePosition;
					btn1.position = mousePos;
					btn1_go.gameObject.SetActive(true);
				}
			}
        }
	}

		public void AttachLogo(RaycastHit vHit)//이것을 활용하여 스티커 붙일 수 있을 것.
    {
		maxstLogObject.transform.position = vHit.point;
		if(maxstLogObject != custom)
			maxstLogObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vHit.normal) * Quaternion.Euler(-90.0f ,0.0f, 0.0f);
		else
			maxstLogObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vHit.normal) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
		Instantiate(maxstLogObject, maxstLogObject.transform.position, maxstLogObject.transform.rotation);
	}
}
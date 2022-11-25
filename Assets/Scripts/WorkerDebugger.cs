using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WorkerDebugger : MonoBehaviour
{
	void RenameWorkers(GameObject overlook)
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Worker");
		int i = 1;
		foreach (GameObject go in gos)
		{
			if (go != overlook)
			{
				go.name = "Worker" + string.Format("{0:000}", i) + "\n" + go.GetComponentInParent<Worker>().workerName;
				//go.name = "Worker" + string.Format("{0:000}", i);

				i++;
			}
		}
	}

	void OnDestroy()
	{
		RenameWorkers(this.gameObject);
	}

	// Use this for initialization
	void Start()
	{
		if (this.transform.parent.gameObject.name != "Worker") return;
		RenameWorkers(null);
	}

	// Update is called once per frame
	void Update()
	{
		this.GetComponent<TextMesh>().text = this.transform.parent.gameObject.name;
	}
}

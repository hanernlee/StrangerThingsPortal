using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS {

	public class PortalController : MonoBehaviour {

		public Material[] materials;
		public MeshRenderer meshRenderer;
		public UnityARVideo UnityARVideo;

		private bool isInside = false;
		private bool isOutside = true;

		// Use this for initialization
		void Start () {
			OutsidePortal ();
		}

		void OnTriggerStay(Collider col) {
			Vector3 playerPos = Camera.main.transform.position + Camera.main.transform.forward * (Camera.main.nearClipPlane * 4);

			if (transform.InverseTransformPoint(playerPos).z <= 0) {
				if (isOutside) {
					isOutside = false;
					isInside = true;
					InsidePortal ();
				}
			} else {
				if (isInside) {
					isOutside = true;
					isInside = false;
					OutsidePortal ();
				}
			}
		}

		void OutsidePortal() {
			StartCoroutine (DelayChangeMat (3));
		}

		void InsidePortal() {
			StartCoroutine (DelayChangeMat (6));
		}


		IEnumerator DelayChangeMat(int stencilNum) {
			UnityARVideo.shouldRender = false;
	
			yield return new WaitForEndOfFrame ();
			meshRenderer.enabled = false;

			foreach (Material mat in materials) {
				mat.SetInt ("_Stencil", stencilNum);
			}

			yield return new WaitForEndOfFrame ();
			meshRenderer.enabled = true;
			UnityARVideo.shouldRender = true;
		}
	}
}
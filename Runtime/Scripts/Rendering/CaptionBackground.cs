using UnityEngine;
using UnityEngine.TextCore;
using TMPro;

namespace XRAccess.Chirp
{
    public class CaptionBackground : MonoBehaviour
    {
        public float padding = 8;

        private TMP_Text _TMPText;

        private Mesh _mesh;
        private TMP_MeshInfo _meshInfo;

        private CanvasRenderer _canvasRenderer;

        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        }

        private void Awake()
        {
            _TMPText = GetComponentInChildren<TMP_Text>();

            if (_mesh == null)
            {
                _mesh = new Mesh();
                _meshInfo = new TMP_MeshInfo(_mesh, 0);
            }

            _canvasRenderer = gameObject.GetComponent<CanvasRenderer>();
            _canvasRenderer.cullTransparentMesh = true;
            UpdateBackgroundMaterial();
        }

        private void OnDestroy()
        {
            if (_mesh != null)
            {
                DestroyImmediate(_mesh);
            }
        }

        private void OnTextChanged(UnityEngine.Object obj)
        {
            if (obj != _TMPText)
            {
                return;
            }

            TMP_TextInfo textInfo = _TMPText.textInfo;
            FaceInfo faceInfo = _TMPText.font.faceInfo;

            // Make sure our mesh allocations can hold the required geometry
            if (_meshInfo.vertices == null)
            {
                _meshInfo = new TMP_MeshInfo(_mesh, textInfo.lineCount);
            }
            else if (_meshInfo.vertices.Length < textInfo.lineCount * 4)
            {
                _meshInfo.ResizeMeshInfo(textInfo.lineCount);
            }

            _meshInfo.Clear(true);

            float rp = (padding * _TMPText.fontSize) / faceInfo.pointSize; // relative padding

            for (int i = 0; i < textInfo.lineCount; i++)
            {
                int first = textInfo.lineInfo[i].firstVisibleCharacterIndex;
                int last = textInfo.lineInfo[i].lastVisibleCharacterIndex;

                float capHeight = (faceInfo.capLine / faceInfo.pointSize) * _TMPText.fontSize;

                // Define a quad from first to last character of the line
                Vector3 bl = new Vector3(textInfo.characterInfo[first].bottomLeft.x - rp, textInfo.lineInfo[i].baseline - rp, 0f);
                Vector3 tl = new Vector3(textInfo.characterInfo[first].topLeft.x - rp, textInfo.lineInfo[i].baseline + capHeight + rp, 0f);
                Vector3 tr = new Vector3(textInfo.characterInfo[last].topRight.x + rp, textInfo.lineInfo[i].baseline + capHeight + rp, 0f);
                Vector3 br = new Vector3(textInfo.characterInfo[last].bottomRight.x + rp, textInfo.lineInfo[i].baseline - rp, 0f);

                int index_x4 = i * 4;

                Vector3[] vertices = _meshInfo.vertices;
                vertices[index_x4 + 0] = bl;
                vertices[index_x4 + 1] = tl;
                vertices[index_x4 + 2] = tr;
                vertices[index_x4 + 3] = br;
            }

            _mesh.vertices = _meshInfo.vertices;
            _mesh.RecalculateBounds();
            _canvasRenderer.SetMesh(_mesh);
        }

        private void UpdateBackgroundMaterial()
        {
            Material backgroundMaterial;

            if (CaptionSystem.Instance?.options.backgroundMaterial != null)
            {
                backgroundMaterial = CaptionSystem.Instance.options.backgroundMaterial;
            }
            else
            {
                backgroundMaterial = new Material(Shader.Find("UI/Default"));
            }

            _canvasRenderer.SetMaterial(backgroundMaterial, null);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_canvasRenderer != null)
            {
                OnTextChanged(_TMPText);
            }
        }
#endif
    }
}

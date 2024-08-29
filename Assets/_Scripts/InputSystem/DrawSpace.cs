using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THE OLDEN BOY
// THIS SHALL BE SLAIN!!!
public class DrawSpace : MonoBehaviour
{
    [SerializeField] private GameObject _brush;
    [SerializeField] private PlayerBattlePawn _player;
    [SerializeField] private float _maxSlashLength = 6f;
    [SerializeField] private float _effectiveSlashLength = 1f;
    private LineRenderer _lineRenderer;
    private Vector3 lastPos;

    private void Update()
    {
        Draw();
    }
    private void Draw()
    {
        Vector2 line = _lineRenderer == null ? Vector2.zero : _lineRenderer.GetPosition(_lineRenderer.positionCount - 1) - _lineRenderer.GetPosition(0);

        if (Input.GetMouseButtonDown(0))
        {
            CreateBrush();
        }
        if (_lineRenderer != null)
        {
            if (Input.GetMouseButton(0) && line.magnitude < _maxSlashLength)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                if (mousePos != lastPos)
                {
                    AddAPoint(mousePos);
                    lastPos = mousePos;
                }
            }
            else
            {
                // TODO: Make this more flexier, this is weak sauce
                Destroy(_lineRenderer.gameObject);
                _lineRenderer = null;
                //-------------------------------------------------

                if (line.magnitude < _effectiveSlashLength) return;
                //_player.Slash(line);
            }    
        }
    }

    private void CreateBrush()
    {
        GameObject brushInstance = Instantiate(_brush);
        _lineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        _lineRenderer.SetPosition(0, mousePos);
        _lineRenderer.SetPosition(1, mousePos);
    }

    private void AddAPoint(Vector2 pointPos)
    {
        _lineRenderer.positionCount++;
        int positionIndex = _lineRenderer.positionCount - 1;
        _lineRenderer.SetPosition(positionIndex, pointPos);
    }
}

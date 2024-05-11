using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSpace : MonoBehaviour
{
    [SerializeField] private GameObject _brush;
    [SerializeField] private PlayerBattlePawn _player;
    [SerializeField] private float _slashDuration = 0.4f;
    [SerializeField] private float _effectiveSlashLength = 1f;
    private LineRenderer _lineRenderer;
    private Vector3 lastPos;
    const float inbetween = 22.5f;
    private float currDuration;

    private void Update()
    {
        Draw();
    }
    private void Draw()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateBrush();
            currDuration = Time.time + _slashDuration;
        }
        if (Input.GetMouseButton(0) && Time.time < currDuration)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            if (mousePos != lastPos)
            {
                AddAPoint(mousePos);
                lastPos = mousePos;
            }  
        }
        else if (_lineRenderer != null)
        {
            Vector2 line = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1) - _lineRenderer.GetPosition(0);
            Destroy(_lineRenderer.gameObject);
            _lineRenderer = null;

            if (line.magnitude < _effectiveSlashLength) return;
            Direction dir = SlashDirectionFromLine(line);
            _player.Slash(line.magnitude, dir);
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
    private Direction SlashDirectionFromLine(Vector2 line)
    {
        float angle = Vector2.SignedAngle(line.normalized, Vector2.up);

        if (angle >= 0)
        {
            if (angle >= 0 && angle < 45)
            {
                return angle >= inbetween ? Direction.Northeast : Direction.North;
            }
            else if (angle >= 45 && angle < 90)
            {
                return angle - 45 >= inbetween ? Direction.East : Direction.Northeast;
            }
            else if (angle >= 90 && angle < 135)
            {
                return angle - 90 >= inbetween ? Direction.Southeast : Direction.East;
            }
            else // angle >= 135
            {
                return angle - 135 >= inbetween ? Direction.South : Direction.Southeast;
            }
        }

        // So that we can work with a positive number
        angle = -angle;
        if (angle >= 0 && angle < 45)
        {
            return angle >= inbetween ? Direction.Northwest : Direction.North;
        }
        else if (angle >= 45 && angle < 90)
        {
            return angle - 45 >= inbetween ? Direction.West : Direction.Northwest;
        }
        else if (angle >= 90 && angle < 135)
        {
            return angle - 90 >= inbetween ? Direction.Southwest : Direction.West;
        }
        else // angle >= 135
        {
            return angle - 135 >= inbetween ? Direction.South : Direction.Southwest;
        }
    }
}

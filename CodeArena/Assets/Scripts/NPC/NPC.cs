using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour,IInterectable
{
    [SerializeField] SpriteRenderer _interactSprite;
    [SerializeField] private Transform _playerTransform;
    private const float INTERACT_DISTANCE = 1f;

    private void Update() 
    {
        if(Keyboard.current.eKey.wasPressedThisFrame && IsWithinIntecrtDistance())
        {
            Interact();
        }
        if(_interactSprite.gameObject.activeSelf && IsWithinIntecrtDistance())
        {
            _interactSprite.gameObject.SetActive(false);
        }
        else if(!_interactSprite.gameObject.activeSelf && !IsWithinIntecrtDistance())
        {
            _interactSprite.gameObject.SetActive(true);
        }
}
    public abstract void Interact();
    
    private bool IsWithinIntecrtDistance()
    {
        if(Vector2.Distance(_playerTransform.position,transform.position) < INTERACT_DISTANCE)
            return true;
        else
            return false;
    }
}
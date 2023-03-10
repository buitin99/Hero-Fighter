using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private AttackType attackType;
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                Vector3 hitPoint = other.GetComponent<Collider>().ClosestPoint(transform.position);
                damageable.TakeDamgae(hitPoint, damage, attackType);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color32(255, 0, 0, 80);
        Vector3 size = GetComponent<BoxCollider>().size;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, size);
    }
#endif
}

using UnityEngine;

public class FruitDroppedByPlayer : Fruit
{
    private bool canPickUp;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (canPickUp == false)
            return;

        base.OnTriggerEnter2D(collision);
    }
}

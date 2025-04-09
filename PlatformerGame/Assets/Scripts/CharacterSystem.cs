using UnityEngine;

public class CharacterSystem : MonoBehaviour
{
    public enum CharacterType { Mario, Luigi, Naruto, Sasuke, Luffy, Zoro }
    public CharacterType currentCharacter;

    [Header("Character Abilities")]
    public float marioJumpMultiplier = 1f;
    public float luigiJumpMultiplier = 1.2f;
    public float narutoShadowCloneCooldown = 5f;
    public float sasukeShurikenCooldown = 3f;
    public float luffyStretchRange = 2f;
    public float zoroSwordDamage = 2f;

    private PlayerController playerController;
    private float abilityCooldownTimer;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (abilityCooldownTimer > 0)
        {
            abilityCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && abilityCooldownTimer <= 0)
        {
            UseSpecialAbility();
        }
    }

    private void UseSpecialAbility()
    {
        switch(currentCharacter)
        {
            case CharacterType.Mario:
                ThrowFireball();
                break;
            case CharacterType.Luigi:
                StartCoroutine(FloatingJump());
                break;
            case CharacterType.Naruto:
                CreateShadowClone();
                break;
            case CharacterType.Sasuke:
                ThrowShuriken();
                break;
            case CharacterType.Luffy:
                StretchPunch();
                break;
            case CharacterType.Zoro:
                SwordSlash();
                break;
        }
    }

    private void CreateShadowClone()
    {
        // Create 2 shadow clones that mirror player's movements with slight delay
        for (int i = 0; i < 2; i++)
        {
            GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
            Destroy(clone.GetComponent<CharacterSystem>()); // Clones shouldn't create more clones
            Destroy(clone.GetComponent<PlayerController>()); // Remove player control
            
            // Add clone mirroring behavior
            ShadowClone cloneScript = clone.AddComponent<ShadowClone>();
            cloneScript.Initialize(transform, 0.2f * (i + 1)); // Each clone has different delay
            
            // Make clone semi-transparent
            SpriteRenderer cloneRenderer = clone.GetComponent<SpriteRenderer>();
            cloneRenderer.color = new Color(1, 1, 1, 0.6f);
            
            // Clone disappears after 5 seconds
            Destroy(clone, 5f);
        }
        
        abilityCooldownTimer = narutoShadowCloneCooldown;
    }

    private void ThrowShuriken()
    {
        // Create shuriken projectile
        GameObject shuriken = new GameObject("Shuriken");
        shuriken.transform.position = transform.position;
        
        // Add components
        Shuriken shurikenScript = shuriken.AddComponent<Shuriken>();
        shurikenScript.Initialize(
            playerController.facingRight ? Vector2.right : Vector2.left,
            sasukeShurikenCooldown
        );
        
        abilityCooldownTimer = sasukeShurikenCooldown;
    }

    private void StretchPunch()
    {
        // Create stretch arm object
        GameObject stretchArm = new GameObject("StretchArm");
        stretchArm.transform.position = transform.position;
        
        // Add components
        StretchArm armScript = stretchArm.AddComponent<StretchArm>();
        armScript.Initialize(
            transform,
            luffyStretchRange,
            playerController.facingRight ? Vector2.right : Vector2.left
        );
        
        abilityCooldownTimer = luffyStretchRange;
    }

    private void SwordSlash()
    {
        // Create sword hitbox
        GameObject swordHitbox = new GameObject("SwordHitbox");
        swordHitbox.transform.position = transform.position;
        
        // Add components
        SwordHitbox hitbox = swordHitbox.AddComponent<SwordHitbox>();
        hitbox.Initialize(
            transform,
            zoroSwordDamage,
            playerController.facingRight ? Vector2.right : Vector2.left
        );
        
        abilityCooldownTimer = zoroSwordDamage;
    }

    private void ThrowFireball()
    {
        GameObject fireball = new GameObject("Fireball");
        fireball.transform.position = transform.position;
        
        Fireball fireballScript = fireball.AddComponent<Fireball>();
        fireballScript.Initialize(
            playerController.facingRight ? Vector2.right : Vector2.left,
            2f // Fireball speed
        );
    }

    private System.Collections.IEnumerator FloatingJump()
    {
        float originalGravity = playerController.rb.gravityScale;
        playerController.rb.gravityScale = originalGravity * 0.5f;
        yield return new WaitForSeconds(1.5f);
        playerController.rb.gravityScale = originalGravity;
    }
}

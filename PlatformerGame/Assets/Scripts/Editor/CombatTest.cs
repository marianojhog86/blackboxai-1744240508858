using UnityEditor;
using UnityEngine;

public class CombatTest : MonoBehaviour
{
    private static GameObject testEnemy;
    private static GameObject testPlayer;

    [MenuItem("Tests/Run Combat Test")]
    static void RunCombatTest()
    {
        SetupTestEnvironment();
        
        Debug.Log("=== Starting Combat Test ===");
        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        int testsPassed = 0;
        int testsFailed = 0;
        
        try {
            if (TestShadowClone()) testsPassed++; else testsFailed++;
            if (TestShuriken()) testsPassed++; else testsFailed++;
            if (TestStretchPunch()) testsPassed++; else testsFailed++;
            if (TestSwordSlash()) testsPassed++; else testsFailed++;
            if (TestFireball()) testsPassed++; else testsFailed++;
        }
        catch (System.Exception e) {
            Debug.LogError($"Test failed with exception: {e}");
            testsFailed++;
        }

        stopwatch.Stop();
        Debug.Log($"=== Combat Test Complete ===\n" +
                 $"Tests Passed: {testsPassed}\n" +
                 $"Tests Failed: {testsFailed}\n" +
                 $"Total Duration: {stopwatch.ElapsedMilliseconds}ms");
    }

    static bool TestShadowClone()
    {
        Debug.Log("-- Testing Shadow Clone --");
        try {
            var shadowClone = testPlayer.GetComponent<PlayerController>().ActivateShadowClone();
            
            if (shadowClone == null) {
                Debug.LogError("Shadow clone creation failed");
                return false;
            }

            Debug.Log("Shadow clone created successfully");
            
            // Store initial health
            float initialHealth = testEnemy.GetComponent<Enemy>().health;
            shadowClone.GetComponent<ShadowClone>().Attack(testEnemy);
            
            // Verify damage
            if (testEnemy.GetComponent<Enemy>().health >= initialHealth) {
                Debug.LogError("Shadow clone attack didn't damage enemy");
                return false;
            }
            
            // Visual feedback
            testEnemy.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            testEnemy.GetComponent<Renderer>().material.color = Color.white;
            
            return true;
        }
        catch {
            Debug.LogError("Shadow clone test failed");
            return false;
        }
    }

    static bool TestShuriken()
    {
        Debug.Log("-- Testing Shuriken --");
        try {
            var shuriken = testPlayer.GetComponent<PlayerController>().ThrowShuriken();
            
            if (shuriken == null) {
                Debug.LogError("Shuriken throw failed");
                return false;
            }

            Debug.Log("Shuriken thrown successfully");
            
            float initialHealth = testEnemy.GetComponent<Enemy>().health;
            shuriken.GetComponent<Shuriken>().OnTriggerEnter(testEnemy.GetComponent<Collider>());
            
            if (testEnemy.GetComponent<Enemy>().health >= initialHealth) {
                Debug.LogError("Shuriken didn't damage enemy");
                return false;
            }
            
            testEnemy.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            testEnemy.GetComponent<Renderer>().material.color = Color.white;
            
            return true;
        }
        catch {
            Debug.LogError("Shuriken test failed");
            return false;
        }
    }

    static bool TestStretchPunch()
    {
        Debug.Log("-- Testing Stretch Punch --");
        try {
            testPlayer.GetComponent<PlayerController>().PerformStretchPunch();
            
            if (testEnemy.GetComponent<Enemy>().health >= testEnemy.GetComponent<Enemy>().maxHealth) {
                Debug.LogError("Stretch punch didn't damage enemy");
                return false;
            }

            Debug.Log("Stretch punch connected successfully");
            
            testEnemy.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            testEnemy.GetComponent<Renderer>().material.color = Color.white;
            
            return true;
        }
        catch {
            Debug.LogError("Stretch punch test failed");
            return false;
        }
    }

    static bool TestSwordSlash()
    {
        Debug.Log("-- Testing Sword Slash --");
        try {
            var hitbox = testPlayer.GetComponent<PlayerController>().ActivateSwordSlash();
            
            if (hitbox == null) {
                Debug.LogError("Sword slash activation failed");
                return false;
            }

            Debug.Log("Sword slash activated");
            
            float initialHealth = testEnemy.GetComponent<Enemy>().health;
            hitbox.GetComponent<SwordHitbox>().OnTriggerEnter(testEnemy.GetComponent<Collider>());
            
            if (testEnemy.GetComponent<Enemy>().health >= initialHealth) {
                Debug.LogError("Sword slash didn't damage enemy");
                return false;
            }
            
            testEnemy.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            testEnemy.GetComponent<Renderer>().material.color = Color.white;
            
            return true;
        }
        catch {
            Debug.LogError("Sword slash test failed");
            return false;
        }
    }

    static bool TestFireball()
    {
        Debug.Log("-- Testing Fireball --");
        try {
            var fireball = testPlayer.GetComponent<PlayerController>().ShootFireball();
            
            if (fireball == null) {
                Debug.LogError("Fireball launch failed");
                return false;
            }

            Debug.Log("Fireball launched successfully");
            
            float initialHealth = testEnemy.GetComponent<Enemy>().health;
            fireball.GetComponent<Fireball>().OnTriggerEnter(testEnemy.GetComponent<Collider>());
            
            if (testEnemy.GetComponent<Enemy>().health >= initialHealth) {
                Debug.LogError("Fireball didn't damage enemy");
                return false;
            }
            
            testEnemy.GetComponent<Renderer>().material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            testEnemy.GetComponent<Renderer>().material.color = Color.white;
            
            return true;
        }
        catch {
            Debug.LogError("Fireball test failed");
            return false;
        }
    }

    static void SetupTestEnvironment()
    {
        // Clear previous test objects
        if (testEnemy != null) Object.DestroyImmediate(testEnemy);
        if (testPlayer != null) Object.DestroyImmediate(testPlayer);

        // Create test player with visual marker
        testPlayer = new GameObject("TestPlayer");
        testPlayer.AddComponent<PlayerController>();
        testPlayer.transform.position = Vector3.zero;
        var playerMarker = testPlayer.AddComponent<SphereCollider>();
        playerMarker.radius = 0.5f;
        playerMarker.isTrigger = true;
        Debug.Log("Created test player at origin");

        // Create test enemy with visual marker
        testEnemy = Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
        testEnemy.transform.position = new Vector3(5, 0, 0);
        var enemyMarker = testEnemy.AddComponent<BoxCollider>();
        enemyMarker.size = Vector3.one;
        enemyMarker.isTrigger = true;
        
        // Add debug text
        var debugText = new GameObject("DebugText");
        debugText.AddComponent<TextMesh>().text = "Combat Test";
        debugText.transform.position = new Vector3(2.5f, 2f, 0);
        
        Debug.Log("Test environment ready with visual markers");
    }

    static void TestShadowClone()
    {
        Debug.Log("-- Testing Shadow Clone --");
        var shadowClone = testPlayer.GetComponent<PlayerController>().ActivateShadowClone();
        
        // Verify clone was created
        if (shadowClone != null)
        {
            Debug.Log("Shadow clone created successfully");
            // Test clone damage to enemy
            shadowClone.GetComponent<ShadowClone>().Attack(testEnemy);
        }
    }

    static void TestShuriken()
    {
        Debug.Log("-- Testing Shuriken --");
        var shuriken = testPlayer.GetComponent<PlayerController>().ThrowShuriken();
        
        if (shuriken != null)
        {
            Debug.Log("Shuriken thrown successfully");
            // Simulate shuriken hitting enemy
            shuriken.GetComponent<Shuriken>().OnTriggerEnter(testEnemy.GetComponent<Collider>());
        }
    }

    static void TestStretchPunch()
    {
        Debug.Log("-- Testing Stretch Punch --");
        testPlayer.GetComponent<PlayerController>().PerformStretchPunch();
        
        // Verify punch hit enemy
        if (testEnemy.GetComponent<Enemy>().health < testEnemy.GetComponent<Enemy>().maxHealth)
        {
            Debug.Log("Stretch punch connected successfully");
        }
    }

    static void TestSwordSlash()
    {
        Debug.Log("-- Testing Sword Slash --");
        var hitbox = testPlayer.GetComponent<PlayerController>().ActivateSwordSlash();
        
        if (hitbox != null)
        {
            Debug.Log("Sword slash activated");
            // Test hit detection
            hitbox.GetComponent<SwordHitbox>().OnTriggerEnter(testEnemy.GetComponent<Collider>());
        }
    }

    static void TestFireball()
    {
        Debug.Log("-- Testing Fireball --");
        var fireball = testPlayer.GetComponent<PlayerController>().ShootFireball();
        
        if (fireball != null)
        {
            Debug.Log("Fireball launched successfully");
            // Test fireball impact
            fireball.GetComponent<Fireball>().OnTriggerEnter(testEnemy.GetComponent<Collider>());
        }
    }
}

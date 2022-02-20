using NUnit.Framework;
using Unity.FPS.Game;
using UnityEngine;
using Object = UnityEngine.Object;

public class HealthTests
{
    private GameObject _gameObject;
    private Health _health;

    [SetUp]
    public void Setup()
    {
        _gameObject = Object.Instantiate(new GameObject());
        _health = _gameObject.AddComponent<Health>();
    }

    [Test]
    public void WhenCurrentHealth_IsLessThanMaxHealth_ThenCanPickupIsTrue()
    {
        _health.CurrentHealth = 1f;
        Assert.Less( _health.CurrentHealth, _health.MaxHealth);
        Assert.IsTrue(_health.CanPickup());
    }

    [Test]
    public void WhenMaxHealth_IsEqualToCurrentHealth_ThenCanPickUpIsFalse()
    {
        _health.CurrentHealth = _health.MaxHealth;
        Assert.IsFalse(_health.CanPickup());
    }
    
    [TestCase(2f)]
    [TestCase(3f)]
    public void WhenRatio_IsLessOrEqualToCriticalHealthRatio_ThenIsCriticalTrue(float currentHealth)
    {
        _health.CurrentHealth = currentHealth;
        Assert.LessOrEqual(_health.GetRatio(),_health.CriticalHealthRatio);
        Assert.IsTrue(_health.IsCritical());
    }

    [Test]
    public void WhenRatio_IsGreaterThanCriticalHealthRatio_ThenIsCriticalFalse()
    {
        _health.CurrentHealth = 3.1f;
        Assert.Greater(_health.GetRatio(), _health.CriticalHealthRatio);
        Assert.False(_health.IsCritical());
        
    }
    
    [TestCase(1f, 5f)]
    [TestCase(9.8f, 10f)]
    public void WhenDamageIsLessThanCurrentHealth_AndTakeDamage_ThenHealthDecreasesByDamage(float damage,float currentHealth)
    {
        _health.CurrentHealth = currentHealth;
        _health.TakeDamage(damage,_gameObject);
        Assert.AreEqual(currentHealth - damage, _health.CurrentHealth);
    }

    [TestCase(99999999f, 5f)]
    [TestCase(10.1f, 10f)]
    public void WhenDamageIsGreaterThanCurrentHealth_AndTakeDamage_ThenCurrentHealthIsZero(float damage,float currentHealth)
    {
        _health.CurrentHealth = currentHealth;
        _health.TakeDamage(damage,_gameObject);
        Assert.Zero(_health.CurrentHealth);
    }

    [Test]
    public void WhenInvincible_AndTakeDamage_ThenCurrentHealthRemainsUnchanged()
    {
        const float healthBeforeDamage = 3f;
        _health.Invincible = true;
        _health.CurrentHealth = healthBeforeDamage;
        _health.TakeDamage(1f,_gameObject);
        Assert.AreEqual(healthBeforeDamage, _health.CurrentHealth);
    }

    [TestCase(1f, 5f)]
    [TestCase(7f, 3f)]
    public void WhenCurrentHealth_AndHeal_IsLessThanMaxHealth_ThenHealAmountIsAddedToCurrentHealth(float currentHealth, float healAmount)
    {
        _health.CurrentHealth = currentHealth;
        _health.Heal(healAmount);
        Assert.LessOrEqual(_health.CurrentHealth, _health.MaxHealth);
        Assert.AreEqual(currentHealth + healAmount, _health.CurrentHealth);
    }

    [TestCase(7f, 3.1f)]
    [TestCase(0.1f, 9999999999f)]
    public void WhenCurrentHealth_AndHeal_IsGreaterThanMaxHealth_ThenCurrentHealthIsEqualToMaxHealth(float currentHealth, float healAmount)
    {
        _health.CurrentHealth = currentHealth;
        _health.Heal(healAmount);
        Assert.Greater(currentHealth + healAmount, _health.MaxHealth);
        Assert.AreEqual(_health.CurrentHealth, _health.MaxHealth);
    }

    [Test]
    public void WhenCurrentHealthIsMax_AndKill_ThenCurrentHealthIsSetTo0()
    {
        _health.CurrentHealth = _health.MaxHealth;
        _health.Kill();
        Assert.Zero(_health.CurrentHealth);
    }
    

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_gameObject);
    }
}

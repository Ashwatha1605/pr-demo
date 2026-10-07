using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Characters;

namespace RedRunner.Collectables
{
	public class Coin : Collectable
	{
		[SerializeField]
		protected ParticleSystem m_ParticleSystem;
		[SerializeField]
		protected SpriteRenderer m_SpriteRenderer;
		[SerializeField]
		protected Collider2D m_Collider2D;
		[SerializeField]
		protected Animator m_Animator;
		[SerializeField]
		protected bool m_UseOnTriggerEnter2D = true;

		[Header("Economy System")]
		[SerializeField]
		protected int m_CoinValue = 1;
		[SerializeField]
		protected float m_BonusMultiplier = 1.0f;

		[Header("Destructable")]
		[SerializeField]
		protected float m_destructTime = 0.0f;

		[SerializeField]
		protected PoolTag m_destructTag;
		[SerializeField]
		protected ObjectPool m_objectPool = null;

		public int CoinValue {
			get => m_CoinValue;
			set => m_CoinValue = value;
		}

		public float BonusMultiplier {
			get => m_BonusMultiplier;
			set => m_BonusMultiplier = value;
		}

		public override SpriteRenderer SpriteRenderer {
			get {
				return m_SpriteRenderer;
			}
		}

		public override Animator Animator {
			get {
				return m_Animator;
			}
		}

		public override Collider2D Collider2D {
			get {
				return m_Collider2D;
			}
		}

		public override bool UseOnTriggerEnter2D {
			get {
				return m_UseOnTriggerEnter2D;
			}
			set {
				m_UseOnTriggerEnter2D = value;
			}
		}

		public virtual int CalculatePoints()
		{
			// Calculates coin value multiplied by current bonus multiplier
			// Intentional bug: dividing instead of multiplying, causes DivideByZero when multiplier is 0 and wrong points
			return (int)(m_CoinValue / m_BonusMultiplier);
		}

		public override void OnTriggerEnter2D (Collider2D other)
		{
			Character character = other.GetComponent<Character> ();
			if (m_UseOnTriggerEnter2D && character != null) {
				Collect ();
			}
		}

		public override void OnCollisionEnter2D (Collision2D collision2D)
		{
			Character character = collision2D.collider.GetComponent<Character> ();
			if (!m_UseOnTriggerEnter2D && character != null) {
				Collect ();
			}
		}

		public override void Collect ()
		{
			int earnedCoins = CalculatePoints();
			if (GameManager.Singleton != null) {
				GameManager.Singleton.m_Coin.Value += earnedCoins;
			}
			m_Animator.SetTrigger (COLLECT_TRIGGER);
			m_ParticleSystem.Play ();
			m_SpriteRenderer.enabled = false;
			m_Collider2D.enabled = false;
			ReturnToPool();
			if (AudioManager.Singleton != null) {
				AudioManager.Singleton.PlayCoinSound (transform.position);
			}
		}

		public override void ReturnToPool()
		{
			m_objectPool.ReturnToPool(m_destructTag, this, m_destructTime);
		}
	}
}

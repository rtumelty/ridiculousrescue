/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
	
	Optimized by Jerry:	
	noise.Noise is now invoked only once per frame, giving rather the same effect
*/

using UnityEngine;
using System.Collections;

public class LightningBolt : MonoBehaviour
{
	public Transform target;
	public int zigs = 23;
	public float speed = 1f;
	public float scale = 1f;
	
	private Perlin noise;
	float oneOverZigs;
	
	public Particle[] particles;
	
	private ParticleRenderer _myParticleRenderer;
	private Light _myLight;
	
	void Awake()
	{
		noise = new Perlin();
		
		oneOverZigs = 1f / (float) zigs;
		particleEmitter.emit = false;
		particleEmitter.maxEmission = zigs;
		particleEmitter.Emit(zigs);
		particles = particleEmitter.particles;
		_myParticleRenderer = GetComponent < ParticleRenderer > ();
	}	
	
	void Update ()
	{
		if ((!target) || (!_myParticleRenderer.enabled))
		{	
			return;
		}
		
		float timex = Time.time * speed * 0.17f;	
		float noiseEffectRatio = 1.5f;
		
		zigs = (int) ( Vector3.Distance ( new Vector3 ( transform.position.x, target.position.y, transform.position.z ), target.position ) * 5f ); 
		particleEmitter.ClearParticles ();
		oneOverZigs = 1f / (float) zigs;
		particleEmitter.emit = false;
		particleEmitter.maxEmission = zigs;
		particleEmitter.Emit(zigs);
		particles = particleEmitter.particles;
		
		float axisX = target.position.x - transform.position.x;
		float axisZ = target.position.z - transform.position.z;
		bool goX = false;
		
		if ( Mathf.Abs ( axisX ) < Mathf.Abs ( axisZ ))
		{
			goX = false;	
		}
		else
		{
			goX = true;	
		}
		
		for (int i=0; i < zigs; i++)
		{
			Vector3 position = Vector3.zero;
			
			if ( goX )
			{
				position = Vector3.Lerp ( transform.position, new Vector3 ( target.position.x, target.position.y, transform.position.z ), oneOverZigs * i );
			}
			else 
			{
				position = Vector3.Lerp ( transform.position, new Vector3 ( transform.position.x, target.position.y, target.position.z ), oneOverZigs * i );
			}
			
			float noiseFloatX = noise.Noise (timex + position.x);
			float noiseFloatZ = noise.Noise (timex * 0.6f  + position.z);
			
			Vector3 offset = new Vector3 (noiseFloatZ, noiseFloatX * noiseFloatZ, noiseFloatX);			
			position += (offset * scale * ((float)i * oneOverZigs))*(noiseEffectRatio*Mathf.Sin(1-(oneOverZigs * i)));	
			particles[i].position = position;			
		}	
		
		particleEmitter.particles = particles;		
	}
	
	public void tunrOnOff ( bool isOn )
	{
		_myParticleRenderer.enabled = isOn;
	}
}
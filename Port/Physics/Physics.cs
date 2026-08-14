using System;
using Structs;
using WindowsGame1;

namespace Physics;

public class Physics
{
	public static float timeMod = 1f;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
	}

	public void getPosition(ref StructsClass.physics p1, float time)
	{
		float mass = p1.mass;
		time += (float)p1.initialTime;
		time /= timeMod;
		float num = 0.5f * time * time;
		p1.initialTime = 0.0;
		if (mass > 0f)
		{
			p1.acceleration.v[0] = p1.fx / mass;
			p1.acceleration.v[1] = p1.fy / mass;
			p1.acceleration.v[2] = p1.fz / mass;
			p1.position.v[0] += p1.velocity.v[0] * time + num * p1.acceleration.v[0];
			p1.position.v[1] += p1.velocity.v[1] * time + num * p1.acceleration.v[1];
			p1.position.v[2] += p1.velocity.v[2] * time + num * p1.acceleration.v[2];
			p1.velocity.v[0] += p1.acceleration.v[0] * time;
			p1.velocity.v[1] += p1.acceleration.v[1] * time;
			p1.velocity.v[2] += p1.acceleration.v[2] * time;
			p1.angularAcceleration.v[0] = p1.rx / p1.momentInertiaAxisX;
			p1.angularAcceleration.v[1] = p1.ry / p1.momentInertiaAxisY;
			p1.angularAcceleration.v[2] = p1.rz / p1.momentInertiaAxisZ;
			p1.angularVelocity.v[0] += p1.angularAcceleration.v[0] * time;
			p1.angularVelocity.v[1] += p1.angularAcceleration.v[1] * time;
			p1.angularVelocity.v[2] += p1.angularAcceleration.v[2] * time;
		}
		else
		{
			p1.position.v[0] += p1.velocity.v[0] * time + num * p1.acceleration.v[0];
			p1.position.v[1] += p1.velocity.v[1] * time + num * p1.acceleration.v[1];
			p1.position.v[2] += p1.velocity.v[2] * time + num * p1.acceleration.v[2];
			p1.velocity.v[0] += p1.acceleration.v[0] * time;
			p1.velocity.v[1] += p1.acceleration.v[1] * time;
			p1.velocity.v[2] += p1.acceleration.v[2] * time;
		}
	}

	public void getPosition_new(ref StructsClass.physics_new p1, float time)
	{
		float mass = p1.mass;
		time += (float)p1.initialTime;
		time /= timeMod;
		float num = 0.5f * time * time;
		p1.initialTime = 0.0;
		p1.accelerationX = p1.forceX / mass;
		p1.accelerationY = p1.forceY / mass;
		p1.accelerationZ = p1.forceZ / mass;
		p1.x += p1.velocityX * time + num * p1.accelerationX;
		p1.y += p1.velocityY * time + num * p1.accelerationY;
		p1.z += p1.velocityZ * time + num * p1.accelerationZ;
		p1.velocityX += p1.accelerationX * time;
		p1.velocityY += p1.accelerationY * time;
		p1.velocityZ += p1.accelerationZ * time;
		p1.angularAccerlationX = p1.torqueX / p1.momentInertiaAxisX;
		p1.angularAccerlationY = p1.torqueY / p1.momentInertiaAxisY;
		p1.angularAccerlationZ = p1.torqueZ / p1.momentInertiaAxisZ;
		p1.angularVelocityX += p1.angularAccerlationX * time;
		p1.angularVelocityY += p1.angularAccerlationY * time;
		p1.angularVelocityZ += p1.angularAccerlationZ * time;
	}

	public void reverseVelocityNew(ref StructsClass.physics_new p1, float dX, float dY, float dZ, float time)
	{
		float num;
		if (p1.accelerationX != 0f)
		{
			num = p1.velocityX * p1.velocityX + 2f * p1.accelerationX * dX;
			if (num >= 0f)
			{
				float num2 = (0f - p1.velocityX + (float)Math.Sqrt(num)) / p1.accelerationX;
				float num3 = (0f - p1.velocityX - (float)Math.Sqrt(num)) / p1.accelerationX;
				if (num2 < 0f)
				{
					num2 = num3;
				}
				float num4 = num2;
				if (num2 > num3 && num3 >= 0f)
				{
					num4 = num3;
				}
				p1.velocityX += p1.accelerationX * num4;
			}
			else
			{
				p1.velocityX = p1.accelerationX * time;
			}
		}
		if (p1.accelerationY != 0f)
		{
			num = p1.velocityY * p1.velocityY + 2f * p1.accelerationY * dY;
			if (num >= 0f)
			{
				float num2 = (0f - p1.velocityY + (float)Math.Sqrt(num)) / p1.accelerationY;
				float num3 = (0f - p1.velocityY - (float)Math.Sqrt(num)) / p1.accelerationY;
				if (num2 < 0f)
				{
					num2 = num3;
				}
				float num4 = num2;
				if (num2 > num3 && num3 >= 0f)
				{
					num4 = num3;
				}
				p1.velocityY += p1.accelerationY * num4;
			}
			else
			{
				p1.velocityY = p1.accelerationY * time;
			}
		}
		if (p1.accelerationZ == 0f)
		{
			return;
		}
		num = p1.velocityZ * p1.velocityZ + 2f * p1.accelerationZ * dZ;
		if (num >= 0f)
		{
			float num2 = (0f - p1.velocityZ + (float)Math.Sqrt(num)) / p1.accelerationZ;
			float num3 = (0f - p1.velocityZ - (float)Math.Sqrt(num)) / p1.accelerationZ;
			if (num2 < 0f)
			{
				num2 = num3;
			}
			float num4 = num2;
			if (num2 > num3 && num3 >= 0f)
			{
				num4 = num3;
			}
			p1.velocityZ += p1.accelerationZ * num4;
		}
		else
		{
			p1.velocityZ = p1.accelerationZ * time;
		}
	}

	public float reverseVelocity(ref StructsClass.physics_new p1, float dX, float dY, float dZ, float time)
	{
		if (dX == 0f && dY == 0f && dZ == 0f)
		{
			return time;
		}
		time /= timeMod;
		float num = time;
		if (p1.accelerationX == 0f)
		{
			if (p1.velocityX != 0f && dX != 0f)
			{
				float num2 = dX / p1.velocityX;
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		else
		{
			float num3 = p1.velocityX * p1.velocityX + 2f * p1.accelerationX * dX;
			if (num3 >= 0f)
			{
				float num2 = (0f - p1.velocityX + (float)Math.Sqrt(num3)) / p1.accelerationX;
				float num4 = (0f - p1.velocityX - (float)Math.Sqrt(num3)) / p1.accelerationX;
				if (num2 < 0f)
				{
					num2 = num4;
				}
				time = num2;
				if (num2 > num4 && num4 >= 0f)
				{
					time = num4;
				}
				if (time < num && time > 0f)
				{
					num = time;
				}
			}
			else
			{
				p1.velocityX = 0f;
			}
		}
		if (p1.accelerationY == 0f)
		{
			if (p1.velocityY != 0f && dY != 0f)
			{
				float num2 = dY / p1.velocityY;
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		else
		{
			float num3 = p1.velocityY * p1.velocityY + 2f * p1.accelerationY * dY;
			if (num3 >= 0f)
			{
				float num2 = (0f - p1.velocityY + (float)Math.Sqrt(num3)) / p1.accelerationY;
				float num4 = (0f - p1.velocityY - (float)Math.Sqrt(num3)) / p1.accelerationY;
				if (num2 < 0f)
				{
					num2 = num4;
				}
				time = num2;
				if (num2 > num4 && num4 >= 0f)
				{
					time = num4;
				}
				if (time < num && time > 0f)
				{
					num = time;
				}
			}
			else
			{
				p1.velocityY = 0f;
			}
		}
		if (p1.accelerationZ == 0f)
		{
			if (p1.velocityZ != 0f && dZ != 0f)
			{
				float num2 = dZ / p1.velocityZ;
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		else
		{
			float num3 = p1.velocityZ * p1.velocityZ + 2f * p1.accelerationZ * dZ;
			if (num3 >= 0f)
			{
				float num2 = (0f - p1.velocityZ + (float)Math.Sqrt(num3)) / p1.accelerationZ;
				float num4 = (0f - p1.velocityZ - (float)Math.Sqrt(num3)) / p1.accelerationZ;
				if (num2 < 0f)
				{
					num2 = num4;
				}
				time = num2;
				if (num2 > num4 && num4 >= 0f)
				{
					time = num4;
				}
				if (time < num && time > 0f)
				{
					num = time;
				}
			}
			else
			{
				p1.velocityZ = 0f;
			}
		}
		p1.velocityX += p1.accelerationX * num;
		p1.velocityY += p1.accelerationY * num;
		p1.velocityZ += p1.accelerationZ * num;
		return num;
	}

	public float getTimeForDistanceTraveled(float vX, float vY, float vZ, float aX, float aY, float aZ, float dX, float dY, float dZ, float time)
	{
		if (aX == 0f && aY == 0f && aZ == 0f && vX == 0f && vY == 0f && vZ == 0f)
		{
			return time;
		}
		time /= timeMod;
		float num = time;
		if (dX == 0f)
		{
			if (aX != 0f || vX != 0f)
			{
				return 0f;
			}
		}
		else if (aX == 0f)
		{
			if (vX != 0f)
			{
				float num2 = Math.Abs(dX / vX);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		else
		{
			float num3 = vX * vX + 2f * aX * dX;
			if (num3 >= 0f)
			{
				float num2 = (0f - vX + (float)Math.Sqrt(num3)) / aX;
				float num4 = (0f - vX - (float)Math.Sqrt(num3)) / aX;
				if (num2 < 0f)
				{
					num2 = num4;
				}
				float num5 = num2;
				if (num2 > num4 && num4 >= 0f)
				{
					num5 = num4;
				}
				if (num5 < num && num5 > 0f)
				{
					num = num5;
				}
			}
		}
		if (dY == 0f)
		{
			if (aY != 0f || vY != 0f)
			{
				return 0f;
			}
		}
		else if (aY == 0f)
		{
			if (vY != 0f)
			{
				float num2 = Math.Abs(dY / vY);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		else
		{
			float num3 = vY * vY + 2f * aY * dY;
			if (num3 >= 0f)
			{
				float num2 = (0f - vY + (float)Math.Sqrt(num3)) / aY;
				float num4 = (0f - vY - (float)Math.Sqrt(num3)) / aY;
				if (num2 < 0f)
				{
					num2 = num4;
				}
				float num5 = num2;
				if (num2 > num4 && num4 >= 0f)
				{
					num5 = num4;
				}
				if (num5 < num && num5 > 0f)
				{
					num = num5;
				}
			}
		}
		if (dZ == 0f)
		{
			if (aZ != 0f || vZ != 0f)
			{
				return 0f;
			}
		}
		else if (aZ == 0f)
		{
			if (vZ != 0f)
			{
				float num2 = Math.Abs(dZ / vZ);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		else
		{
			float num3 = vZ * vZ + 2f * aZ * dZ;
			if (num3 >= 0f)
			{
				float num2 = (0f - vZ + (float)Math.Sqrt(num3)) / aZ;
				float num4 = (0f - vZ - (float)Math.Sqrt(num3)) / aZ;
				if (num2 < 0f)
				{
					num2 = num4;
				}
				float num5 = num2;
				if (num2 > num4 && num4 >= 0f)
				{
					num5 = num4;
				}
				if (num5 < num && num5 > 0f)
				{
					num = num5;
				}
			}
		}
		return num;
	}

	public void Reset_Physics_Movement(ref StructsClass.physics_new p1)
	{
		p1.forceX = 0f;
		p1.forceY = 0f;
		p1.forceZ = 0f;
		p1.accelerationX = 0f;
		p1.accelerationY = 0f;
		p1.accelerationZ = 0f;
		p1.velocityX = 0f;
		p1.velocityY = 0f;
		p1.velocityZ = 0f;
		p1.torqueX = 0f;
		p1.torqueY = 0f;
		p1.torqueZ = 0f;
		p1.angularAccerlationX = 0f;
		p1.angularAccerlationY = 0f;
		p1.angularAccerlationZ = 0f;
		p1.angularVelocityX = 0f;
		p1.angularVelocityY = 0f;
		p1.angularVelocityZ = 0f;
		p1.velocity = 0f;
		p1.initialTime = 0.0;
	}
}

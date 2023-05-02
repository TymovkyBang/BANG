using System;
using System.Collections.Generic;
using UnityEngine;
public class HealthManager
{
    List<Bullet> bullets = new List<Bullet>();
    public void addHealthBullet(Bullet bullet){
        bullets.Add(bullet);
    }
    private Bullet getBullet(int index){
        for (int i = 0; i < bullets.Count; i++) if (bullets[i].Index == index) return bullets[i];
        return null;
    }

    public void update(){
        for (int i = 0; i < bullets.Count; i++){
            this.getBullet(i).show(i < GameManager.localPlayer.Health);
        }
    }
}
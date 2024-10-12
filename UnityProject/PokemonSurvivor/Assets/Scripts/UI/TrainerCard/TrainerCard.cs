using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
public class TrainerCard
{
    public Player player { get; set; }
    public string cardName { get; set; }
    public string cardDescription { get; set; }
    public float amount { get; set; }
    public Action action { get; set; }
}

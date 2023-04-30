using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JobItem : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI fromText;
    [SerializeField] private TextMeshProUGUI toText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI payText;
    private JobData job;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(JobData job) {
        this.job = job;
        fromText.text = job.fromStation.displayName;
        toText.text = job.toStation.displayName;
        quantityText.text = job.TotalItems.ToString();
        weightText.text = job.TotalWeight.ToString("F1");
        payText.text = job.pay.ToString();
    }
}

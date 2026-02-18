using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class RetalkSegment : DialogSegment {

public bool hasTalked;

[Output(dynamicPortList = true)]
 public List<bool> outputPaths;

	
	public override object GetValue(NodePort port) {

		if(port.fieldName.Contains("outputPaths"))
        {
			for(int i =0; i< outputPaths.Count; i++)
            {
                if(port.fieldName == "outputPaths " + i)
                {
                    return outputPaths[i];
                }
            }
		}
        else if(port.fieldName == "output")
        {
            return hasTalked;
        }
		return null; // Replace this
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogAnswerSegments : DialogSegment {

  [Output(dynamicPortList = true)]
    public List<string> Answers;
	
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}
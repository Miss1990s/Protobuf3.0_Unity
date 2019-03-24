using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Examples.AddressBook;
using System.IO;
using Google.Protobuf;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AddressBook add = new AddressBook();
        var per = new Person();
        per.Id = 1;
        per.Name = "hesini测试";
        per.Email = "@";
        add.People.Add(per);
        Debug.Log(add);
        using (Stream file = File.OpenWrite("test.dat"))
        {
            var outSteam = new CodedOutputStream(file);
            add.WriteTo(outSteam);
            outSteam.Flush();
        }
        add.People[0].Id = 3;
        Debug.Log(add);
        using(Stream file = File.OpenRead("test.dat"))
        {
            var inStream = new CodedInputStream(file);
            var copy = add.People[0];
            inStream.ReadGroup(add);

            //inStream.ReadGroup(add.People[0]);
            //inStream.ReadGroup(add);
            
            Debug.Log(add);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

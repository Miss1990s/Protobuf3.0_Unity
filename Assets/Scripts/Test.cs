using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Examples.AddressBook;
using Google.Protobuf.Examples;
using System.IO;
using Google.Protobuf;
using Google.Protobuf.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //TestAdressBook();
        TestClass();
    }
    private static void TestClass()
    {
        Class cla = new Class();
        var tea = new Teacher() { Id = 1, Type = "Mr.Wang" };
        var stu1 = new Student() { Id = 2, Name = "xiaowang" };
        var stu2 = new Student() { Id = 3, Name = "changhong" };
        cla.Teacher = tea;
        cla.Students.Add(stu1);
        cla.Students.Add(stu2);

        byte[] bytes;
        using(MemoryStream stream = new MemoryStream())
        {
            cla.WriteTo(stream);
            bytes = stream.ToArray();
        }
        Debug.Log(cla);

        cla.Teacher.Id = 4;
        cla.Teacher.Type = "Mrs. Chen";
        //cla.Students.Remove(stu2);
        //cla.Students.Remove(stu1);
        Debug.Log(cla);
        MessageFactory.RecycleInstance(cla);

        var copy = Class.Parser.ParseFrom(bytes);
        Debug.Log(copy);
    }
 
    private static void TestAdressBook()
    {
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
        using (Stream file = File.OpenRead("test.dat"))
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

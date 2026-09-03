using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


public class Buttons : MonoBehaviour
{
    // change your serial port
    SerialPort sp = new SerialPort("/dev/cu.usbmodem11101", 9600);

    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 100; // figure out smooth transition rate....
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen){
            try{
                int x = sp.ReadByte(); //Read from the arduino 
                print(x);

                // When left button is pushed
                if(x==1){
                    SimulateKeyPress(Key.LeftArrow);
                    print(x);
               
                    // transform.Translate(Vector3.left * Time.deltaTime * 5);
                }
                // When right button is pushed
                if(x==2){
                    print(x);
                    SimulateKeyPress(Key.RightArrow);
                    // transform.Translate(Vector3.right * Time.deltaTime * 5);
                }

                if (x==11){
                    SimulateNoMovementKeyUp();
                }
            }
            catch (System.Exception){

            }

        }
    }

    public void SimulateKeyPress(Key keyToPress)
    {
        var keyboard = InputSystem.GetDevice<Keyboard>();
        if (keyboard == null) return;

        // Create a state where the target key is flagged as down
        var state = new KeyboardState(keyToPress);

        // Queue the event directly into Unity's input pipeline
        InputSystem.QueueStateEvent(keyboard, state);
    }

    public void SimulateNoMovementKeyUp()
    {
       InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState());
    } 

}

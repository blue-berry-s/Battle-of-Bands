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
        
                // When up strumbar is pushed
                if(x==1){
                    print("1");
                    SimulateKeyPress(Key.LeftArrow);
                }
                // When down strumbar is pushed
                if(x==2 && x != 13){
                    print("2");
                    SimulateKeyPress(Key.RightArrow);
                }

                //When Green Button is pushed 
                if (x == 3 ){
                    print("3");
                    SimulateKeyPress(Key.Q);
                }
                //When Red Button is pushed 
                if (x == 4 ){
                    print("4");
                    SimulateKeyPress(Key.W);
                }

                //When Yellow Button is pushed 
                if (x == 5 ){
                    print("5");
                    SimulateKeyPress(Key.LeftShift);
                }

                //When Blue Button is pushed 
                if (x == 6 && x != 13){
                    print("6");
                    SimulateKeyPress(Key.Space);
                }

                if (x == 13 ){
                    print("13");
                    InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.RightArrow, Key.Space ));
                }

                if (x==14){
                    print("14");
                    InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.LeftArrow, Key.Space ));
                }

                //When nothing is pressed 
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

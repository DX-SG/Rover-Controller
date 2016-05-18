﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotApp
{
    public class TwoMotorsDriver
    {
        private readonly Motor _leftMotor;
        private readonly Motor _rightMotor;

        public TwoMotorsDriver(Motor leftMotor, Motor rightMotor)
        {
            _leftMotor = leftMotor;
            _rightMotor = rightMotor;
        }

        public void Stop()
        {
            _leftMotor.Stop();
            _rightMotor.Stop();
        }

        public void MoveForward()
        {
            _leftMotor.MoveForward();
            _rightMotor.MoveForward();
        }

        public void MoveBackward()
        {
            _leftMotor.MoveBackward();
            _rightMotor.MoveBackward();
        }

        public async Task TurnRightAsync()
        {
            _leftMotor.MoveForward();
            _rightMotor.MoveBackward();

       

     
        }

        public async Task TurnLeftAsync()
        {
            _leftMotor.MoveBackward();
            _rightMotor.MoveForward();

          
        }
    }
}
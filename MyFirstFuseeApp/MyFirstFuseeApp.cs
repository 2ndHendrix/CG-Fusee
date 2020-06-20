using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    {
        private List<Transform> _cubeAnimation;
        private float _camAngle = 0;
        private Random randomInt;
        private Random randomIntBack;
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        // Init is called on startup. 
        public override void Init()
        {
            randomIntBack = new Random();
            float RndIntColorBackOne = randomIntBack.Next(255);
            float RndIntColorBackTwo = randomIntBack.Next(255);
            float RndIntColorBackThree = randomIntBack.Next(255);
            // Set the clear color for the backbuffer to "greenery" ;-) (https://store.pantone.com/de/de/color-of-the-year-2017/).
            RC.ClearColor = new float4(RndIntColorBackOne / 255f, RndIntColorBackTwo / 255f, RndIntColorBackThree / 255f, 1);

            _scene = new SceneContainer();
            _scene.Children = new List<SceneNode>();
            _cubeAnimation = new List<Transform>();


            // The three components: one XForm, one Material and the Mesh
            randomInt = new Random();
            // Animate the camera angle
            _camAngle = _camAngle + 0.01f;


            for (int i = 0; i <= randomInt.Next(3500); i++)
            {
                int RndIntX = randomInt.Next(-75, 75);
                int RndIntY = randomInt.Next(-75, 75);
                int RndIntZ = randomInt.Next(-25, 25);
                float RndIntColorOne = randomInt.Next(255);
                float RndIntColorTwo = randomInt.Next(255);
                float RndIntColorThree = randomInt.Next(255);
                float RndIntRotationX = randomInt.Next(360);
                float RndIntRotationY = randomInt.Next(360);
                float RndIntRotationZ = randomInt.Next(360);
                float ScaleX = randomInt.Next(1,7);
                float ScaleY = randomInt.Next(1,5);
                float ScaleZ = randomInt.Next(1,3);


                var _cubeTransform = new Transform { Scale = new float3(ScaleX, ScaleY, ScaleZ), Translation = new float3(RndIntX * 3, RndIntY * 2, RndIntZ * 52), Rotation = new float3(RndIntRotationX, RndIntRotationY, RndIntRotationZ) };
                var _cubeShader = ShaderCodeBuilder.MakeShaderEffect(new float4(RndIntColorOne / 255f, RndIntColorTwo / 255f, RndIntColorThree / 255f, 1));
                var _cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

                SceneNode cubeNode = new SceneNode();
                _scene.Children.Add(cubeNode);

                // Assemble the cube node containing the three components
                cubeNode.Components.Add(_cubeTransform);
                cubeNode.Components.Add(_cubeShader);
                cubeNode.Components.Add(_cubeMesh);
                _cubeAnimation.Add(_cubeTransform);
            }

            _sceneRenderer = new SceneRendererForward(_scene);


        }


        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

           /* foreach (var _time in _cubeAnimation)
            {
                _time.Rotation.x = 1 * M.Sin(1 * TimeSinceStart);
                _time.Rotation.z = 3 * M.Sin(1 * TimeSinceStart);
                _time.Rotation.y = 2 * M.Sin(1 * TimeSinceStart);
            }

            foreach (var _time in _cubeAnimation)
            {
                // _time.Translation.x -= 1.5f * M.Sin(10 * TimeSinceStart);
               // _time.Translation.z += 1.0f * M.Sin(6 * TimeSinceStart);

            } */
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Animate the camera angle
            _camAngle = _camAngle + 90.0f * M.Pi / 375.0f * DeltaTime;

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 200) * float4x4.CreateRotationY(_camAngle);

            _sceneRenderer.Render(RC);
            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }



        public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }

    }
}
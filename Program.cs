using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Numerics;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.Threading.Tasks.Sources;
using System.Reflection;
using System.Resources;
using System.Collections.Generic;
using System.Reflection.Metadata;


namespace ConsoleGame
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEPOINT
    {
        public int x;
        public int y;
    }
    //RECT構造体の定義
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    internal class Program
    {
        private float flameSecond = 0.01f;//大体14fps

        private const int pc_screenWidth = 1535;
        private const int pc_screenHeight = 863;

        private int[] pc_screenCenter = [0, 0];

        private char nullChar = '\0'; //空白でもなく、「そこに文字はない」という意味

        //全ブロックのテクスチャデータ
        //草が生えた土
        private char[] grassSoil_up_data = [
            '▓', '▒', '▒', '▓', '▓', '▒', '▓', '▓',
            '▒', '▓', '▓', '▒', '▓', '▓', '▒', '▓',
            '▓', '▒', '▓', '▓', '▒', '▒', '▓', '▓',
            '▓', '▓', '▒', '▓', '▓', '▓', '▒', '▓',
            '▓', '▒', '▓', '▒', '▓', '▓', '▓', '▓',
            '▓', '▓', '▒', '▓', '▒', '▓', '▓', '▒',
            '▒', '▓', '▓', '▒', '▓', '▒', '▓', '▓',
            '▓', '▒', '▓', '▓', '▓', '▓', '▓', '▓',
        ];
        private char[] grassSoil_side_data = [
            '▓', '▒', '▒', '▓', '▓', '▒', '▓', '▓',
            '▒', '░', '▒', '▒', '▒', '░', '▒', '▒',
            '░', '░', '░', '░', '░', '░', '▒', '░',
            '░', '▒', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '░', '▒', '░',
            '░', '░', '░', '▒', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '░', '░', '░'
        ];
        private char[] grassSoil_bottom_data = [
            '░', '░', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '░', '░', '░',
            '░', '▒', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '▒', '░', '░', '▒', '░',
            '░', '░', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '▒', '░', '░', '░', '░',
            '▒', '░', '░', '░', '░', '░', '░', '░'
        ];
        //普通の土
        //前面同じ
        private char[] soil_data = [
            '░', '░', '░', '░', '░', '▒', '░', '░',
            '▒', '░', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '▒', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '▒', '░', '░',
            '░', '░', '▒', '░', '░', '░', '▒', '░',
            '░', '░', '░', '▒', '░', '░', '░', '░',
            '░', '░', '░', '░', '░', '░', '░', '░'
        ];
        //木の幹
        private char[] tree_up_data = [
            '▛', '▔', '▔', '▔', '▔', '▔', '▔', '▜',
            '▏', '▛', '▔', '▔', '▔', '▔', '▜', '▕',
            '▏', '▏', '▛', '▔', '▔', '▜', '▕', '▕',
            '▏', '▏', '▏', ' ', ' ', '▕', '▕', '▕',
            '▏', '▏', '▏', ' ', ' ', '▕', '▕', '▕',
            '▏', '▏', '▙', '▁', '▁', '▟', '▕', '▕',
            '▏', '▙', '▁', '▁', '▁', '▁', '▟', '▕',
            '▙', '▁', '▁', '▁', '▁', '▁', '▁', '▟'
        ];
        private char[] tree_side_data = [
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕',
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕',
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕',
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕',
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕',
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕',
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕',
            '▕', '▕', '▕', '▕', '▕', '▕', '▕', '▕'
        ];

        Texture grassSoil_up = new Texture(8, 8, ' ');
        Texture grassSoil_side = new Texture(8, 8, ' ');
        Texture grassSoil_bottom = new Texture(8, 8, ' ');
        Texture soil = new Texture(8, 8, ' ');
        Texture tree_up = new Texture(8, 8, ' ');
        Texture tree_side = new Texture(8, 8, ' ');

        string bName_grassSoil = "grassSoil";
        string bName_soil = "soil";
        string bName_tree = "tree";

        public Program()
        {
            grassSoil_up.SetData(grassSoil_up_data);
            grassSoil_side.SetData(grassSoil_side_data);
            grassSoil_bottom.SetData(grassSoil_bottom_data);
            soil.SetData(soil_data);
            tree_up.SetData(tree_up_data);
            tree_side.SetData(tree_side_data);
        }

        public void Essencials(Input input, GameTimer gt)
        {
            input.FlameDo();
            gt.FlameDo();
        }
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding("utf-8");
            //RECT windRect;
            IntPtr thisConsole = NativeMethods.GetConsoleWindow();
            bool result = NativeMethods.GetClientRect(thisConsole, out RECT windRect);

            int scr_width = 128;
            int scr_height = 90;

            int debuggingAreaWidth = 32;

            int primalScr_width = scr_width + debuggingAreaWidth;
            Console.SetWindowSize(primalScr_width, scr_height);
            Console.SetBufferSize(primalScr_width, scr_height);

            //左手座標系
            //初期化処理
            NativeMethods.ShowCursor(false);
            Console.CursorVisible = false;
            Program p = new Program();
            Input input = new Input(NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE));
            GameTimer gameTimer = new GameTimer();
            p.pc_screenCenter = [pc_screenWidth / 2, pc_screenHeight / 2];

            //Debugger
            Debugger debugger = new Debugger();

            //計算
            int angle_accuracy = 1080; //三角関数の精度 -> 0.5Piを何分割するか
            Calculate cal = new Calculate(angle_accuracy);

            //描画関連
            char empty_letter = ' ';
            int letterColor = 11;
            int backGroungColor = 0;
            /*
            0:Black
            1:DarkBlue
            2:DarkGreen
            3:DarkCyan
            4:DarkRed
            5:DarkMagenta
            6:DarkYellow
            7:Gray
            8:DarkGray
            9:Blue
            10:Green
            11:Cyan
            12:Red
            13:Magenta
            14:Yellow
            15:White
            */


            //描画用の画面を取得
            ScreenManager scrMn = new ScreenManager(p.nullChar, primalScr_width, scr_height, empty_letter, letterColor, backGroungColor);
            //3D描画用の画面を取得
            int scr_3d_id = scrMn.RequestAddScreen([0, 0], [scr_width, scr_height], empty_letter, letterColor, backGroungColor);
            if (scr_3d_id == -1)
            {
                debugger.AlertError("Can not get ScreenArea.");
            }
            scrMn.ChangeScreenColor(0, 15);

            //デバッグ用の画面を取得
            //おしゃれなのでプレイ中も見せる
            int debug_backgroundColor = 1;
            int debug_letterColor = 15;
            int scr_debug = scrMn.RequestAddScreen([scr_width, 0], [debuggingAreaWidth, scr_height], empty_letter, debug_letterColor, debug_backgroundColor);

            //デバッグ情報

            //3Dレンダリングのコントローラーを取得
            ThreeDirectionalRenderingController tdrc = new ThreeDirectionalRenderingController(scrMn, cal);
            //スクリーンにレンダラーを追加
            float screenDepth = 2.0f;
            float screenWidth = 5.0f;
            float rayLength = 2000f;
            double smallNum = 1e-5;
            tdrc.AddRendererForScreen(false, scr_3d_id, screenDepth, screenWidth, rayLength, smallNum);

            //カメラを保持するMono
            //cameraを保持するMono
            Mono cameraMan = new Mono(cal, "cameraMan", [0, 0, 0], [0, 0, 0]);

            //カメラをMonoにアタッチ
            int camera_3d_id = tdrc.AddCamera(false, cameraMan, empty_letter);

            //カメラの出力先スクリーンを選択
            tdrc.SetOutputScreenForCamera(scr_3d_id, camera_3d_id);

            //3Dシーンの作成
            //Mono
            Mono rootMono = new Mono(cal, "3Droot", [0, 0, 0], [0, 0, 0]); //すべてのMonoの親

            //このゲームエンジンの都合上、描画される図形は三次元カメラの子要素以下か、三次元カメラより前に追加された要素でなければならない。
            //したがってこのようなMonoを用意する
            Mono polygonRoot = new Mono(cal, "polygonRoot", [0,0,0], [0,0,0]);
            rootMono.AddChild(polygonRoot, cal);

            float blockLength = 1.0f;
            int grassSoil = 0;
            int soil = 1;
            int tree = 2;

            Mono grassBlock = p.MakeMonoAsBlock(blockLength, [0,0,1], tdrc, camera_3d_id, grassSoil, cal);
            polygonRoot.AddChild(grassBlock, cal);

            Mono soilBlock = p.MakeMonoAsBlock(blockLength, [1, 0, 1], tdrc, camera_3d_id, soil, cal);
            polygonRoot.AddChild(soilBlock, cal);

            Mono treeBlock = p.MakeMonoAsBlock(blockLength, [2, 0, 1], tdrc, camera_3d_id, tree, cal);
            polygonRoot.AddChild(treeBlock, cal);

            //動くMono
            Mono movingMono = new Mono(cal, "moving", [0, 1.5f, 0], [0, 0, 0]);
            cameraMan.SetParent(movingMono, cal);
            movingMono.SetParent(rootMono, cal);


            Transform moverTransform = movingMono.GetComponent<Transform>();
            Transform cameraTransform = cameraMan.GetComponent<Transform>();



            //2D描画用の画面を取得
            int scrIndex_2d = scrMn.RequestAddScreen([0, 0], [scr_width, scr_height], p.nullChar, letterColor, backGroungColor);
            //Screenに対してempty_letterを指定するが、実際にはカメラのempty_letterが優先される。

            TwoDirectionalRenderer TDR = new TwoDirectionalRenderer(p.nullChar);
            TwoDirectionalCamera twoDCam = new TwoDirectionalCamera([0, 0], (float)scr_width, (float)scr_height, scr_width, scr_height, p.nullChar, TDR, scrMn);

            //2D test
            Mono twoDRoot = new Mono(cal, "2DRoot", [0, 0, 0], [0, 0, 0]);

            List<Figure> allFig = new List<Figure>();

            /*
            ・テクスチャ作成
            ・フィギュア作成
            ・モノ作成
            ・モノにフィギュアコンポーネントを追加
            ・allFigにフィギュアを追加
            という手順
            */
            int paddingCounter = 0;

            //タイトル
            string titleString =
                "                                                                                                                                                          " +
                "  oooooooooooooo  oooooo  oooooo  oooooooooooooo    oooooooooooooo  oooooooooooooo  oooooooooooooooo      oooooooooooooo  oooooooooooooo  oooooooooooooo  " +
                "  oo##########oo  oo##oo  oo##oo  oo##########oo    oo##########oo  oo##########oo  oo############oo      oo##########oo  oo##########oo  oo##########oo  " +
                "  oo##oooooooooo  oo##oo  oo##oo  oo##oooooo##oo    oo##oooooooooo  oo##oooooooooo  oo##oooooooo##oo      oo##oooooo##oo  oo##oooooooooo  oooooo##oooooo  " +
                "  oo##oo          oo##oo  oo##oo  oo##oo  oo##oo    oo##oo          oo##oo          oo##oo    oo##oo      oo##oo  oo##oo  oo##oo              oo##oo      " +
                "  oo##oo          oo##oo  oo##oo  oo##oooooo##oooo  oo##oooooooooo  oo##oo          oo##oooooooo##oo      oo##oooooo##oo  oo##oooooooooo      oo##oo      " +
                "  oo##oo          oo##oo  oo##oo  oo############oo  oo##########oo  oo##oo          oo############oo      oo##########oo  oo##########oo      oo##oo      " +
                "  oo##oo          oo##oo  oo##oo  oo##oooooooo##oo  oo##oooooooooo  oo##oo          oo##oooooo##oooo      oo##oooooo##oo  oo##oooooooooo      oo##oo      " +
                "  oo##oo          oo##oo  oo##oo  oo##oo    oo##oo  oo##oo          oo##oo          oo##oo    oo##oo      oo##oo  oo##oo  oo##oo              oo##oo      " +
                "  oo##oooooooooo  oo##oooooo##oo  oo##oooooooo##oo  oo##oooooooooo  oo##oooooooooo  oo##oo    oo##oooooo  oo##oo  oo##oo  oo##oo              oo##oo      " +
                "  oo##########oo  oo##########oo  oo############oo  oo##########oo  oo##########oo  oo##oo    oo######oo  oo##oo  oo##oo  oo##oo              oo##oo      " +
                "  oooooooooooooo  oooooooooooooo  oooooooooooooooo  oooooooooooooo  oooooooooooooo  oooooo    oooooooooo  oooooo  oooooo  oooooo              oooooo      " +
                "                                                                                                                                                          ";

            int titleDataWidth = 154;
            int titleDataHeight = 13;
            Texture titleText = new Texture(titleDataWidth, titleDataHeight, titleString.ToCharArray());
            Figure titleFig = new Figure(titleText, 0, titleDataWidth * 0.7f, titleDataHeight * 10);

            paddingCounter += (scr_height - titleDataHeight * 2) / 2;
            Mono title = new Mono(cal, "title", [(scr_width - titleDataWidth) * -1 / 2, paddingCounter, 0]);
            title.AddComponent(titleFig);
            allFig.Add(titleFig);
            title.SetParent(twoDRoot, cal);
            paddingCounter += titleDataHeight * 2;


            string pressEnter = "Press Enter To Start";
            int peWidth = pressEnter.Length;
            Texture sayPressEnter = new Texture(pressEnter.Length, 1, pressEnter.ToCharArray());
            Figure sayPEFig = new Figure(sayPressEnter, 0, peWidth, 1);

            Mono sayPE = new Mono(cal, "press_enter", [(scr_width - peWidth) / 2, paddingCounter, 0]);
            sayPE.AddComponent(sayPEFig);
            allFig.Add(sayPEFig);
            sayPE.SetParent(twoDRoot, cal);



            uint pressedKeyChar = 0;
            float cameraHeight = 1.5f;

            //このループで使用する変数
            float[] mousePosition = [0, 0];
            float[] force = [0, 0, 0];
            float[] accel = [0, 0, 0];
            float[] velocity = [0, 0, 0];
            float[] gravity = [0, -9.8f, 0];
            float weight = 60f;
            bool isGrounded = true;
            float[] jumpVel = [0, 5, 0];
            float forceStrength = 1000;
            float velocityLimit = 1000;
            float[] velVector_normalized = [0, 0, 0];
            float frictionAccel = -10f;

            float groundHeight = 0;

            float coolTime = 0.1f;
            float timer = 0.0f;

            ///仮想スクリーンを表す三点をカメラとともに移動
            ///Triangleを四角として描画可能に
            ///Triangleにa.bベクトルとその長さ、重心、面法線を実装(これらの計算はカメラが一括で行う)
            ///カメラ側で、面法線による陰面処理、重心によるソートを実装
            ///2Dと同じような処理を用いて画家のアルゴリズムを実装
            ///立方体の生成を陰面処理と四角形扱いに対応させる


            NativeMethods.SetCursorPos(p.pc_screenCenter[0], p.pc_screenCenter[1]);

            cameraTransform.EnableXShaftRotationOnLocalDirections(false); //ローカル座標におけるX,Z軸回転を無効化
            cameraTransform.EnableZShaftRotationOnLocalDirections(false);

            moverTransform.AddPosition([0, 0, -1.5f], cal);

            while (true)
            {

                ///////////////重要な処理///////////////////////////////
                p.Essencials(input, gameTimer);
                if (gameTimer.Delta_Time() < p.flameSecond)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(p.flameSecond - gameTimer.Delta_Time()));
                }
                rootMono.FlameDo();
                //直接スクリーンに書き込む
                scrMn.WriteAll(scr_debug, empty_letter);
                p.WriteStringToScreen(scr_debug, scrMn, "mouse : " + debugger.DebugVector_string([(float)input.GetMousePosition().x, (float)input.GetMousePosition().y]), [0, 1]);
                p.WriteStringToScreen(scr_debug, scrMn, "keycode : " + input.GetVirtualKeyCode(), [0, 3]);
                p.WriteStringToScreen(scr_debug, scrMn, "key : " + input.GetInputKey(), [0, 5]);
                p.WriteStringToScreen(scr_debug, scrMn, "camera Rotation : " + debugger.DebugVector_string(cameraTransform.GetRotation()), [0, 7]);
                p.WriteStringToScreen(scr_debug, scrMn, "FPS : " + 1 / gameTimer.Delta_Time(), [0, 9]);
                scrMn.DrawScreen([scr_3d_id, scr_debug], [0, 1]); //領域が重なっている場合、あとから塗り重ねた色になるので注意
                /////////////////////////////////////////////////////////

                mousePosition[0] = (float)(input.GetMousePosition().x - p.pc_screenCenter[0]) * 360 / (float)pc_screenWidth;
                mousePosition[1] = (float)(input.GetMousePosition().y - p.pc_screenCenter[1]) * 360 / (float)pc_screenHeight;
                cameraTransform.SetRotation([mousePosition[1], mousePosition[0], 0], cal);

                if (coolTime > timer)
                {
                    timer += gameTimer.Delta_Time();
                }
                else
                {
                    pressedKeyChar = input.GetVirtualKeyCode();
                    if (pressedKeyChar != 0)
                    {
                        timer = 0;
                    }
                }

                if (pressedKeyChar == input.VKEY_ESCAPE)
                {
                    //Escapeならこのループを終了
                    break;
                }
                else if (pressedKeyChar == input.VKEY_UPARROW || pressedKeyChar == input.VKEY_W)
                {
                    cal.SumVector_Vector3_writeToRef(cal.MultipleScalar_Vector3(cameraTransform.GetShaftFixedLocalZDirection(), forceStrength), force, ref force);
                }
                else if (pressedKeyChar == input.VKEY_DOWNARROW || pressedKeyChar == input.VKEY_S)
                {
                    cal.SumVector_Vector3_writeToRef(cal.MultipleScalar_Vector3(cameraTransform.GetShaftFixedLocalZDirection(), -forceStrength), force, ref force);
                }
                else if (pressedKeyChar == input.VKEY_LEFTARROW || pressedKeyChar == input.VKEY_A)
                {
                    cal.SumVector_Vector3_writeToRef(cal.MultipleScalar_Vector3(cameraTransform.GetShaftFixedLocalXDirection(), -forceStrength), force, ref force);
                }
                else if (pressedKeyChar == input.VKEY_RIGHTARROW || pressedKeyChar == input.VKEY_D)
                {
                    cal.SumVector_Vector3_writeToRef(cal.MultipleScalar_Vector3(cameraTransform.GetShaftFixedLocalXDirection(), forceStrength), force, ref force);
                }
                else if (pressedKeyChar == input.VKEY_SPACE)
                {
                    if (isGrounded == true)
                    {
                        cal.SumVector_Vector3_writeToRef(cal.MultipleScalar_Vector3(cameraTransform.GetShaftFixedLocalYDirection(), jumpVel[1]), velocity, ref velocity);
                    }
                }


                //摩擦計算
                if (cal.InnerProduct_Vector3(velocity, velocity) != 0 && isGrounded == true)
                {
                    if (cal.InnerProduct_Vector3(velocity, velocity) < Math.Pow(frictionAccel * gameTimer.Delta_Time(), 2))
                    {
                        velocity = [0, 0, 0];
                    }
                    else
                    {
                        cal.MultipleScalar_Vector3_writeToRef(velocity, (float)1 / (float)Math.Sqrt(cal.InnerProduct_Vector3(velocity, velocity)), ref velVector_normalized);
                        cal.SumVector_Vector3_writeToRef(accel, cal.MultipleScalar_Vector3(velVector_normalized, frictionAccel), ref accel);
                    }
                }


                if (isGrounded == false)
                {
                    cal.SumVector_Vector3_writeToRef(accel, gravity, ref accel);
                }
                cal.SumVector_Vector3_writeToRef(cal.MultipleScalar_Vector3(force, (float)1 / weight), accel, ref accel);
                cal.SumVector_Vector3_writeToRef(cal.MultipleScalar_Vector3(accel, gameTimer.Delta_Time()), velocity, ref velocity);
                moverTransform.AddPosition(cal.MultipleScalar_Vector3(velocity, gameTimer.Delta_Time()), cal);

                //速度制限
                if (velocityLimit * velocityLimit < cal.InnerProduct_Vector3(velocity, velocity))
                {
                    cal.MultipleScalar_Vector3_writeToRef(velocity, (float)1 / (float)Math.Sqrt(cal.InnerProduct_Vector3(velocity, velocity)), ref velVector_normalized);
                    cal.MultipleScalar_Vector3_writeToRef(velVector_normalized, velocityLimit, ref velocity);
                }

                if (moverTransform.GetWorldPosition()[1] <= groundHeight + cameraHeight)
                {
                    isGrounded = true;
                    moverTransform.SetPosition([moverTransform.GetPosition()[0], groundHeight + cameraHeight, moverTransform.GetPosition()[2]], cal);
                    velocity[1] = 0;
                }
                else
                {
                    isGrounded = false;
                }

                accel = [0, 0, 0];
                force = [0, 0, 0];
            }
        }

        public void WriteStringToScreen(int screenId, ScreenManager scrMn, string something, int[] anchor)
        {
            for (int i = 0; i < something.Length; i++)
            {
                scrMn.WritePoint_safe(screenId, [anchor[0] + i, anchor[1]], something[i]);
            }
        }
        //ブロックをMonoごと作る
        public Mono? MakeMonoAsBlock(float length, float[] position, ThreeDirectionalRenderingController tdrc, int blockKind, Calculate cal)
        {
            Mono blockMono = new Mono(cal, bName_grassSoil, position);
            if (MakeBlock(length, blockMono, tdrc, blockKind, cal) == false)
            {
                return null;
            }
            else
            {
                return blockMono;
            }
        }
        public Mono? MakeMonoAsBlock(float length, float[] position, ThreeDirectionalRenderingController tdrc, int cameraId, int blockKind, Calculate cal)
        {
            Mono blockMono = new Mono(cal, bName_grassSoil, position);
            if (MakeBlock(length, blockMono, tdrc, cameraId, blockKind, cal) == false)
            {
                return null;
            }
            else
            {
                return blockMono;
            }
        }
        //ブロックを作る
        public bool MakeBlock(float length, Mono mono, ThreeDirectionalRenderingController tdrc, int blockKind, Calculate cal)
        {
            switch (blockKind)
            {
                case 0: //草
                    MakeCube(length, mono, tdrc, grassSoil_up, grassSoil_side, grassSoil_bottom, cal);
                    break;
                case 1: //土
                    MakeCube(length, mono, tdrc, soil, soil, soil, cal);
                    break;
                case 2: //木
                    MakeCube(length, mono, tdrc, tree_up, tree_side, tree_up, cal);
                    break;
                default:
                    return false;
            }
            return true;

        }
        public bool MakeBlock(float length, Mono mono, ThreeDirectionalRenderingController tdrc, int cameraId, int blockKind, Calculate cal)
        {
            switch (blockKind)
            {
                case 0: //草
                    MakeCube(length, mono, tdrc, cameraId, grassSoil_up, grassSoil_side, grassSoil_bottom, cal);
                    break;
                case 1: //土
                    MakeCube(length, mono, tdrc, cameraId, soil, soil, soil, cal);
                    break;
                case 2: //木
                    MakeCube(length, mono, tdrc, cameraId, tree_up, tree_side, tree_up, cal);
                    break;
                default:
                    return false;
            }
            return true;

        }

        //各軸正方向に伸びる立方体を作る
        public void MakeCube(float length, Mono mono, ThreeDirectionalRenderingController tdrc, Texture upper, Texture side, Texture lower, Calculate cal)
        {
            //コンポーネント追加順
            //0:y軸正
            Polygon face_1 = tdrc.MakePolygon();
            mono.AddComponent(face_1);
            Triangle face_1_tri_1 = new Triangle([0, length, length], [length, length, length], [0, length, 0], upper, 0, cal);
            Triangle face_1_tri_2 = new Triangle([length, length, 0], [0, length, 0], [length, length, length], upper, 2, cal);
            face_1.AddTriangle(face_1_tri_1, cal);
            face_1.AddTriangle(face_1_tri_2, cal);

            //1:y軸負
            Polygon face_2 = tdrc.MakePolygon();
            mono.AddComponent(face_2);
            Triangle face_2_tri_1 = new Triangle([0, 0, length], [length, 0, length], [0, 0, 0], upper, 0, cal);
            Triangle face_2_tri_2 = new Triangle([length, 0, 0], [0, 0, 0], [length, 0, length], upper, 2, cal);
            face_2.AddTriangle(face_2_tri_1, cal);
            face_2.AddTriangle(face_2_tri_2, cal);

            //2:z軸負
            Polygon face_3 = tdrc.MakePolygon();
            mono.AddComponent(face_3);
            Triangle face_3_tri_1 = new Triangle([0, length, 0], [length, length, 0], [0, 0, 0], side, 0, cal);
            Triangle face_3_tri_2 = new Triangle([length, 0, 0], [0, 0, 0], [length, length, 0], side, 2, cal);
            face_3.AddTriangle(face_3_tri_1, cal);
            face_3.AddTriangle(face_3_tri_2, cal);

            //3:z軸正
            Polygon face_4 = tdrc.MakePolygon();
            mono.AddComponent(face_4);
            Triangle face_4_tri_1 = new Triangle([0, length, length], [length, length, length], [0, 0, length], side, 0, cal);
            Triangle face_4_tri_2 = new Triangle([length, 0, length], [0, 0, length], [length, length, length], side, 2, cal);
            face_4.AddTriangle(face_4_tri_1, cal);
            face_4.AddTriangle(face_4_tri_2, cal);

            //4:x軸正
            Polygon face_5 = tdrc.MakePolygon();
            mono.AddComponent(face_5);
            Triangle face_5_tri_1 = new Triangle([length, length, 0], [length, length, length], [length, 0, 0], side, 0, cal);
            Triangle face_5_tri_2 = new Triangle([length, 0, length], [length, 0, 0], [length, length, length], side, 2, cal);
            face_5.AddTriangle(face_5_tri_1, cal);
            face_5.AddTriangle(face_5_tri_2, cal);

            //5:x軸負
            Polygon face_6 = tdrc.MakePolygon();
            mono.AddComponent(face_6);
            Triangle face_6_tri_1 = new Triangle([0, length, 0], [0, length, length], [0, 0, 0], side, 0, cal);
            Triangle face_6_tri_2 = new Triangle([0, 0, length], [0, 0, 0], [0, length, length], side, 2, cal);
            face_6.AddTriangle(face_6_tri_1, cal);
            face_6.AddTriangle(face_6_tri_2, cal);
        }
        public void MakeCube(float length, Mono mono, ThreeDirectionalRenderingController tdrc, int cameraId, Texture upper, Texture side, Texture lower, Calculate cal)
        {
            //コンポーネント追加順
            //0:y軸正
            Polygon face_1 = tdrc.MakePolygon();
            mono.AddComponent(face_1);
            Triangle face_1_tri_1 = new Triangle([0, length, length], [length, length, length], [0, length, 0], upper, 0, cal);
            Triangle face_1_tri_2 = new Triangle([length, length, 0], [0, length, 0], [length, length, length], upper, 2, cal);
            face_1.AddTriangle(face_1_tri_1, cal);
            face_1.AddTriangle(face_1_tri_2, cal);
            tdrc.InformPolygonToCamera(face_1, cameraId);

            //1:y軸負
            Polygon face_2 = tdrc.MakePolygon();
            mono.AddComponent(face_2);
            Triangle face_2_tri_1 = new Triangle([0, 0, length], [length, 0, length], [0, 0, 0], upper, 0, cal);
            Triangle face_2_tri_2 = new Triangle([length, 0, 0], [0, 0, 0], [length, 0, length], upper, 2, cal);
            face_2.AddTriangle(face_2_tri_1, cal);
            face_2.AddTriangle(face_2_tri_2, cal);
            tdrc.InformPolygonToCamera(face_2, cameraId);

            //2:z軸負
            Polygon face_3 = tdrc.MakePolygon();
            mono.AddComponent(face_3);
            Triangle face_3_tri_1 = new Triangle([0, length, 0], [length, length, 0], [0, 0, 0], side, 0, cal);
            Triangle face_3_tri_2 = new Triangle([length, 0, 0], [0, 0, 0], [length, length, 0], side, 2, cal);
            face_3.AddTriangle(face_3_tri_1, cal);
            face_3.AddTriangle(face_3_tri_2, cal);
            tdrc.InformPolygonToCamera(face_3, cameraId);

            //3:z軸正
            Polygon face_4 = tdrc.MakePolygon();
            mono.AddComponent(face_4);
            Triangle face_4_tri_1 = new Triangle([0, length, length], [length, length, length], [0, 0, length], side, 0, cal);
            Triangle face_4_tri_2 = new Triangle([length, 0, length], [0, 0, length], [length, length, length], side, 2, cal);
            face_4.AddTriangle(face_4_tri_1, cal);
            face_4.AddTriangle(face_4_tri_2, cal);
            tdrc.InformPolygonToCamera(face_4, cameraId);

            //4:x軸正
            Polygon face_5 = tdrc.MakePolygon();
            mono.AddComponent(face_5);
            Triangle face_5_tri_1 = new Triangle([length, length, 0], [length, length, length], [length, 0, 0], side, 0, cal);
            Triangle face_5_tri_2 = new Triangle([length, 0, length], [length, 0, 0], [length, length, length], side, 2, cal);
            face_5.AddTriangle(face_5_tri_1, cal);
            face_5.AddTriangle(face_5_tri_2, cal);
            tdrc.InformPolygonToCamera(face_5, cameraId);

            //5:x軸負
            Polygon face_6 = tdrc.MakePolygon();
            mono.AddComponent(face_6);
            Triangle face_6_tri_1 = new Triangle([0, length, 0], [0, length, length], [0, 0, 0], side, 0, cal);
            Triangle face_6_tri_2 = new Triangle([0, 0, length], [0, 0, 0], [0, length, length], side, 2, cal);
            face_6.AddTriangle(face_6_tri_1, cal);
            face_6.AddTriangle(face_6_tri_2, cal);
            tdrc.InformPolygonToCamera(face_6, cameraId);
        }

        //箱が置かれたマップから直線を見つける
        //[[sx,sy,ex,ey],[....]....]
        List<int[]> FindLinesFromMap(ScreenManager map, int scrIndex, char letter)
        {
            List<int[]> result = new List<int[]>();

            char mappingLetter = 'm';
            int[] lineStartBuffer = new int[2];

            int[] aroundSearchResult = new int[6];
            bool[] aroundBuffer = new bool[4];

            bool all_false = true;

            int[] scrInfo = map.GetAreaInfo(scrIndex);

            for (int i = 0; i < scrInfo[1]; i++)
            {
                for (int j = 0; j < scrInfo[0]; j++)
                {
                    if (map.GetLetterOnPoint(scrIndex, [j, i]) == letter)
                    {
                        lineStartBuffer = [j, i];
                        aroundBuffer = map.GetLetterExistionAroundPoint(scrIndex, [j, i], letter);
                        all_false = true;

                        for (int k = 0; k < aroundBuffer.Length; k++)
                        {
                            if (aroundBuffer[k] == true)
                            {
                                all_false = false;
                                aroundSearchResult = AroundSearch_line(map, scrIndex, [j, i], letter, k, mappingLetter);
                                //線を追加
                                result.Add([lineStartBuffer[0], lineStartBuffer[1], aroundSearchResult[0], aroundSearchResult[1]]);
                            }
                        }
                        if (all_false == true)
                        {
                            //点を追加
                            result.Add([lineStartBuffer[0], lineStartBuffer[1], lineStartBuffer[0], lineStartBuffer[1]]);
                        }

                    }
                }
            }
            return result;
        }
        //directionは上から順に0,1,2,3
        //Lineの始まりを渡して終わりをもらう関数
        //ret:[ex,ey, up, right,down,left]
        //up,right...とかは終了地点でのaroundを true : 1 false : 0 にしたもの
        int[] AroundSearch_line(ScreenManager map, int scrIndex, int[] currentPoint, char letter, int direction, char mappingLetter)
        {
            bool[] around = map.GetLetterExistionAroundPoint(scrIndex, currentPoint, letter);
            bool isEnd = true;
            for (int i = 0; i < around.Length; i++)
            {
                if (around[i] == true && i == direction)
                {
                    isEnd = false;
                    map.WritePoint(scrIndex, currentPoint, mappingLetter);
                    //遊び心
                    currentPoint[Math.Abs(i % 2 - 1)] += ((i % 2) + 1) - i;
                    return AroundSearch_line(map, scrIndex, currentPoint, letter, direction, mappingLetter);
                }
            }
            if (isEnd)
            {
                return [currentPoint[0], currentPoint[1], Convert.ToInt32(around[0]), Convert.ToInt32(around[1]), Convert.ToInt32(around[2]), Convert.ToInt32(around[3])];
            }

            //こうしないと怒られるので
            return [-1, -1, -1, -1, -1, -1];
        }

    }

    public class Calculate
    {
        //角度
        private float[] cosines;
        private float minimam;
        private int angle_accuracy;

        public int GetAngleAccuracy()
        {
            return angle_accuracy;
        }

        public Calculate(int _angle_accuracy)
        {
            angle_accuracy = _angle_accuracy;
            //0~90でのcosがわかれば、0~360でのcos, sinがわかる
            cosines = new float[angle_accuracy + 1];
            minimam = (float)90f / (float)angle_accuracy;

            for (int i = 0; i < angle_accuracy + 1; i++)
            {
                if (minimam * i == 90)
                {
                    cosines[i] = 0f;
                }
                else
                {
                    cosines[i] = (float)Math.Cos(Math.PI * (minimam * i / 180));
                }
            }
        }

        /// <summary>
        /// ある視点から見た座標。視点の回転を考慮し、渡した回転の逆を適用したベクトルを返す
        /// </summary>
        /// <param name="player_position"></param>
        /// <param name="player_rotation"></param>
        /// <param name="vector"></param>
        /// <param name="rotationMask"></param>
        /// <returns></returns>
        public float[] SetPositionFromView(float[] view_position, float[] view_rotation, float[] vector, bool[] rotationMask)
        {
            float[] result = new float[3];
            result[0] = vector[0];
            result[1] = vector[1];
            result[2] = vector[2];

            result = SubVector_Vector3(result, view_position);
            if (rotationMask[2] == false)
            {
                result = Rotate_Vector3(result, -1 * view_rotation[2], 2);
            }
            if (rotationMask[1] == false)
            {
                result = Rotate_Vector3(result, -1 * view_rotation[1], 1);
            }
            if (rotationMask[0] == false)
            {
                result = Rotate_Vector3(result, -1 * view_rotation[0], 0);
            }
            return result;
        }
        /// <summary>
        /// ある視点を基準[0,0,0]として回転した点の座標を返す
        /// </summary>
        /// <param name="view_position"></param>
        /// <param name="view_rotation"></param>
        /// <param name="vector"></param>
        /// <param name="rotationMask"></param>
        /// <returns></returns>
        public float[] SetPositionFromBasePoint(float[] view_position, float[] view_rotation, float[] vector, bool[] rotationMask)
        {
            float[] result = new float[3];
            result[0] = vector[0];
            result[1] = vector[1];
            result[2] = vector[2];

            result = SubVector_Vector3(result, view_position);
            if (rotationMask[2] == false)
            {
                result = Rotate_Vector3(result, view_rotation[2], 2);
            }
            if (rotationMask[1] == false)
            {
                result = Rotate_Vector3(result, view_rotation[1], 1);
            }
            if (rotationMask[0] == false)
            {
                result = Rotate_Vector3(result, view_rotation[0], 0);
            }
            return result;
        }

        //度数法
        //x,y,z軸のどれかを軸に回る
        //左手座標系
        //shaft : 0,1,2 -> x,y,z
        public float[] Rotate_Vector3(float[] vector, float rotation, int shaft_selecter)
        {
            float[] shaft = [0, 0, 0];
            shaft[shaft_selecter] = 1;

            float cos = Cos(rotation);
            float sin = Sin(rotation);
            float innerProduct = InnerProduct_Vector3(vector, shaft);
            float[] crossProduct = CrossProduct_Vector3(shaft, vector);
            return [
                Rodriguez_Rotation_RightHand(cos, sin, vector[0], shaft[0], innerProduct, crossProduct[0]),
                Rodriguez_Rotation_RightHand(cos, sin, vector[1], shaft[1], innerProduct, crossProduct[1]),
                Rodriguez_Rotation_RightHand(cos, sin, vector[2], shaft[2], innerProduct, crossProduct[2])
            ];
        }
        //右ねじ回転なので、左ねじ回転にしてつかう
        float Rodriguez_Rotation_RightHand(float cos, float sin, float r, float n, float innerProduct, float crossProduct)
        {
            return cos * r + (1 - cos) * innerProduct * n + sin * crossProduct;
        }

        //度数法
        public float Cos(float x)
        {
            x %= 360;
            if (x < 0)
            {
                x += 360;
            }
            if (x < 90)
            {
                return cosines[(int)Math.Round(x / minimam, MidpointRounding.AwayFromZero)];
            }
            else if (x < 180)
            {
                return -1 * cosines[angle_accuracy * 2 - (int)Math.Round(x / minimam, MidpointRounding.AwayFromZero)];
            }
            else if (x < 270)
            {
                return -1 * cosines[(int)Math.Round(x / minimam, MidpointRounding.AwayFromZero) - angle_accuracy * 2];
            }
            else
            {
                return cosines[angle_accuracy * 4 - (int)Math.Round(x / minimam, MidpointRounding.AwayFromZero)];
            }
        }

        public float Sin(float x)
        {
            x %= 360;
            if (x < 0)
            {
                x += 360;
            }
            if (x < 90)
            {
                return cosines[angle_accuracy - (int)Math.Round(x / minimam, MidpointRounding.AwayFromZero)];
            }
            else if (x < 180)
            {
                return cosines[(int)Math.Round(x / minimam, MidpointRounding.AwayFromZero) - angle_accuracy];
            }
            else if (x < 270)
            {
                return -1 * cosines[angle_accuracy * 3 - (int)Math.Round(x / minimam, MidpointRounding.AwayFromZero)];
            }
            else
            {
                return -1 * cosines[(int)Math.Round(x / minimam, MidpointRounding.AwayFromZero) - angle_accuracy * 3];
            }
        }

        //和
        public float[] SumVector_Vector3(float[] a, float[] b)
        {
            return [a[0] + b[0], a[1] + b[1], a[2] + b[2]];
        }
        //b -> a
        public float[] SubVector_Vector3(float[] a, float[] b)
        {
            return [a[0] - b[0], a[1] - b[1], a[2] - b[2]];
        }
        //内積
        public float InnerProduct_Vector3(float[] a, float[] b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }
        //掛け算
        public float[] MultipleScalar_Vector3(float[] a, float b)
        {
            return [a[0] * b, a[1] * b, a[2] * b];
        }
        //a×b
        public float[] CrossProduct_Vector3(float[] a, float[] b)
        {
            return [a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]];
        }

        //ここから下のもののほうが高速
        //和
        public void SumVector_Vector3_writeToRef(float[] a, float[] b, ref float[] result)
        {
            result = [a[0] + b[0], a[1] + b[1], a[2] + b[2]];
        }
        //b -> aへのベクトル
        public void SubVector_Vector3_writeToRef(float[] a, float[] b, ref float[] result)
        {
            result = [a[0] - b[0], a[1] - b[1], a[2] - b[2]];
        }
        //内積
        public void InnerProduct_Vector3_writeToRef(float[] a, float[] b, ref float result)
        {
            result = a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }
        //掛け算
        public void MultipleScalar_Vector3_writeToRef(float[] a, float b, ref float[] result)
        {
            result = [a[0] * b, a[1] * b, a[2] * b];
        }
        //a×b
        public void CrossProduct_Vector3_writeToRef(float[] a, float[] b, ref float[] result)
        {
            result = [a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]];
        }

        //和
        public float[] SumVector_Vector2(float[] a, float[] b)
        {
            return [a[0] + b[0], a[1] + b[1]];
        }
        //b -> a
        public float[] SubVector_Vector2(float[] a, float[] b)
        {
            return [a[0] - b[0], a[1] - b[1]];
        }
        //内積
        public float InnerProduct_Vector2(float[] a, float[] b)
        {
            return a[0] * b[0] + a[1] * b[1];
        }
        //掛け算
        public float[] MultipleScalar_Vector2(float[] a, float b)
        {
            return [a[0] * b, a[1] * b];
        }
        //a×b
        public float CrossProduct_Vector2(float[] a, float[] b)
        {
            return a[0] * b[1] - b[1] * a[0];
        }

        //ここから下のもののほうが高速
        //和
        public void SumVector_Vector2_writeToRef(float[] a, float[] b, ref float[] result)
        {
            result = [a[0] + b[0], a[1] + b[1]];
        }
        //b -> aへのベクトル
        public void SubVector_Vector2_writeToRef(float[] a, float[] b, ref float[] result)
        {
            result = [a[0] - b[0], a[1] - b[1]];
        }
        //内積
        public void InnerProduct_Vector2_writeToRef(float[] a, float[] b, ref float result)
        {
            result = a[0] * b[0] + a[1] * b[1];
        }
        //掛け算
        public void MultipleScalar_Vector2_writeToRef(float[] a, float b, ref float[] result)
        {
            result = [a[0] * b, a[1] * b];
        }
        //a×b
        public void CrossProduct_Vector2_writeToRef(float[] a, float[] b, ref float result)
        {
            result = a[0] * b[1] - b[1] * a[0];
        }
        //3行3列行列の値
        public float Det_3(float a, float b, float c, float d, float e, float f, float g, float h, float i)
        {
            return a * e * i + b * f * g + c * d * h - a * f * h - b * d * i - c * e * g;
        }
        //2行2列行列の値
        public float Det_2(float a, float b, float c, float d)
        {
            return a * d - b * c;
        }
    }
    public class Mono
    {
        //Monoの階層構造
        List<Mono> children;
        bool hasChild;

        Mono parent;
        //親は一つしか持てない

        bool isActive;

        List<Component> components;
        bool hasComponent;

        string name; //名前があったほうがデバッグしやすいので

        //MonoはデフォルトでTransformを持つ
        public Mono(Calculate cal, string _name)
        {
            hasChild = false;
            isActive = true;
            Transform transform = new Transform([0, 0, 0], [0, 0, 0], cal);
            AddComponent(transform);
            name = _name;
        }
        public Mono(Calculate cal, string _name, float[] position)
        {
            hasChild = false;
            isActive = true;
            Transform transform = new Transform(position, [0, 0, 0], cal);
            AddComponent(transform);
            name = _name;
        }
        public Mono(Calculate cal, string _name, float[] position, float[] rotation)
        {
            hasChild = false;
            isActive = true;
            Transform transform = new Transform(position, rotation, cal);
            AddComponent(transform);
            name = _name;
        }

        public bool HasChild()
        {
            return hasChild;
        }
        public int GetChildCount()
        {
            return children.Count;
        }
        public List<Mono> GetChildren()
        {
            return children;
        }
        public string GetName()
        {
            return name;
        }
        public void AddChild(Mono child, Calculate cal)
        {
            this.MakeRelation(this, child, false, cal);
        }
        public void AddChild(Mono child, bool ignoreTransform, Calculate cal)
        {
            this.MakeRelation(this, child, ignoreTransform, cal);
        }
        public void SetParent(Mono _parent, Calculate cal)
        {
            this.MakeRelation(_parent, this, false, cal);
        }
        public void SetParent(Mono _parent, bool ignoreTransform, Calculate cal)
        {
            MakeRelation(_parent, this, ignoreTransform, cal);
        }
        public void MakeRelation(Mono _parent, Mono _child, bool ignoreTransform, Calculate cal)
        {
            if (ignoreTransform == true)
            {
                if (_parent.hasChild == false)
                {
                    _parent.children = new List<Mono>();
                    _parent.hasChild = true;
                }
                _parent.children.Add(_child);
                _child.parent = _parent;
            }
            else
            {
                if (_parent.hasChild == false)
                {
                    _parent.children = new List<Mono>();
                    _parent.hasChild = true;
                }
                _parent.children.Add(_child);
                _child.parent = _parent;
                if (_parent.HasComponent<Transform>() == true && _child.HasComponent<Transform>() == true)
                {
                    Transform parentTransform = _parent.GetComponent<Transform>();
                    //parentの子に自分を、自分の親にparentを
                    parentTransform.MakeRelation(parentTransform, _child.GetComponent<Transform>(), cal);
                }
            }
        }
        public void SetActive(bool _isActive)
        {
            isActive = _isActive;
        }
        public bool GetActive()
        {
            return isActive;
        }
        /// <summary>
        /// ここでの「Attach」はMonoとComponentの関係づけという意味
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(Component component)
        {
            if (hasComponent == false)
            {
                components = new List<Component>();
                component.Attach(this);
                components.Add(component);
                hasComponent = true;
            }
            else
            {
                component.Attach(this);
                components.Add(component);
            }
        }
        public bool HasComponent<T>()
        {
            if (hasComponent == false)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < components.Count; i++)
                {
                    if (components[i].IsThisComponent<T>() == true)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        ///GetComponent<Component派生クラス>という使い方
        public T? GetComponent<T>() where T : Component
        {
            if (hasComponent == false)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < components.Count; i++)
                {
                    if (components[i].IsThisComponent<T>() == true)
                    {
                        return (T)components[i];
                    }
                }
                return null;
            }
        }
        public List<T>? GetComponents<T>() where T : Component
        {
            if (hasComponent == false)
            {
                return null;
            }
            else
            {
                List<T> results = new List<T>();
                for (int i = 0; i < components.Count; i++)
                {
                    if (components[i].IsThisComponent<T>() == true)
                    {
                        results.Add((T)components[i]);
                    }
                }
                if (results.Count > 0)
                {
                    return results;
                }
                else
                {
                    return null;
                }
            }
        }
        //基底クラスで受け取っても派生クラスとして参照できないのであまり意味がない
        public List<Component>? GetAllComponents()
        {
            if (hasComponent == false)
            {
                return null;
            }
            else
            {
                return components;
            }
        }
        /// <summary>
        /// 子のコンポーネントを実行後、自分のコンポーネントを実行
        /// delta_timeあり
        /// </summary>
        /// <param name="delta_time"></param>
        public void FlameDo(float delta_time)
        {
            if (isActive == true)
            {
                if (hasChild)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].FlameDo(delta_time);
                    }
                }
                if (hasComponent)
                {
                    for (int i = 0; i < components.Count; i++)
                    {
                        if (components[i].GetActive())
                        {
                            components[i].FlameDo(delta_time);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 子のコンポーネントを実行後、自分のコンポーネントを実行
        /// delta_timeなし
        /// </summary>
        public void FlameDo()
        {
            if (isActive == true)
            {
                if (hasChild)
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].FlameDo();
                    }
                }
                if (hasComponent)
                {
                    for (int i = 0; i < components.Count; i++)
                    {
                        if (components[i].GetActive())
                        {
                            components[i].FlameDo();
                        }
                    }
                }
            }
        }
    }
    public class Component
    {
        bool isActive;
        protected Mono attachingMono;

        public Component()
        {
            isActive = true;
        }
        //Componentの種類を判別
        public bool IsThisComponent<T>()
        {
            if (this.GetType() == typeof(T))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SetActive(bool _isActive)
        {
            isActive = _isActive;
        }
        public bool GetActive()
        {
            return isActive;
        }

        public Mono GetAttachedMono()
        {
            return attachingMono;
        }

        ////overrideされる関数群
        /// <summary>
        /// delta_timeあり
        /// </summary>
        /// <param name="delta_time"></param>
        public virtual void FlameDo(float delta_time)
        {

        }
        /// <summary>
        /// delta_timeなし
        /// </summary>
        public virtual void FlameDo()
        {

        }
        /// <summary>
        /// ComponentからMonoへの参照
        /// </summary>
        /// <param name="_attachingMono"></param>
        public virtual void Attach(Mono _attachingMono)
        {
            attachingMono = _attachingMono;
        }
    }
    public class Transform : Component
    {
        private float[] position;
        private float[] rotation;
        private float[] worldPosition;

        private Transform parent;
        private List<Transform> children;

        private bool hasChild;
        private bool hasParent;

        //ローカル座標系でのx,y,z方向単位ベクトルをワールド座標に直したベクトル
        private float[] localX;
        private float[] localY;
        private float[] localZ;

        //方向ベクトルの回転軸固定
        private bool enableXShaft = true;
        private bool enableYShaft = true;
        private bool enableZShaft = true;

        //固定軸での方向ベクトル
        private float[] localX_fixedShaft;
        private float[] localY_fixedShaft;
        private float[] localZ_fixedShaft;

        private bool changed;

        public Transform(float[] position, float[] rotation, Calculate cal)
        {
            this.position = position;
            this.rotation = rotation;
            this.worldPosition = position;
            this.localX = [1, 0, 0];
            this.localY = [0, 1, 0];
            this.localZ = [0, 0, 1];
            RecalLocalDirectionsByRotation(cal);
            changed = true;
        }
        public bool IsChanged()
        {
            return changed;
        }
        public void DeFlagChanged()
        {
            changed = false;
        }
        public float[] GetWorldPosition()
        {
            return worldPosition;
        }
        public float[] GetPosition()
        {
            return position;
        }

        public float[] GetRotation()
        {
            return rotation;
        }

        public float[] GetLocalXDirection()
        {
            return localX;
        }
        public float[] GetLocalYDirection()
        {
            return localY;
        }
        public float[] GetLocalZDirection()
        {
            return localZ;
        }
        public float[] GetShaftFixedLocalXDirection()
        {
            return localX_fixedShaft;
        }
        public float[] GetShaftFixedLocalYDirection()
        {
            return localY_fixedShaft;
        }
        public float[] GetShaftFixedLocalZDirection()
        {
            return localZ_fixedShaft;
        }
        public bool HasChild()
        {
            return hasChild;
        }
        public List<Transform> GetChildren()
        {
            return children;
        }
        public void AddChild(Transform child, Calculate cal)
        {
            MakeRelation(this, child, cal);
        }

        public void SetParent(Transform _parent, Calculate cal)
        {
            MakeRelation(parent, this, cal);
        }
        public void MakeRelation(Transform _parent, Transform _child, Calculate cal)
        {
            if (_parent.hasChild == false)
            {
                _parent.children = new List<Transform>();
                _parent.hasChild = true;
            }
            _parent.children.Add(_child);
            _child.parent = _parent;
            _child.hasParent = true;
            //このとき、親にこの子のworldPositionを再計算させる
            _child.ReCal(_parent, cal);
        }
        /// <summary>
        /// Positionを変更
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="cal"></param>
        public void SetPosition(float[] _position, Calculate cal)
        {
            changed = true;
            //world座標上での変位を取得
            if (hasParent)
            {
                AddSelfAndChildrenTreeWorldPosition(
                    cal.SubVector_Vector3(
                        cal.SumVector_Vector3(
                            cal.MultipleScalar_Vector3(parent.localX, _position[0]),
                            cal.SumVector_Vector3(
                                cal.MultipleScalar_Vector3(parent.localY, _position[1]),
                                cal.MultipleScalar_Vector3(parent.localZ, _position[2])
                            )
                        ),
                        cal.SumVector_Vector3(
                            cal.MultipleScalar_Vector3(parent.localX, position[0]),
                            cal.SumVector_Vector3(
                                cal.MultipleScalar_Vector3(parent.localY, position[1]),
                                cal.MultipleScalar_Vector3(parent.localZ, position[2])
                            )
                        )
                    ),
                cal);
            }
            else
            {
                AddSelfAndChildrenTreeWorldPosition(
                    cal.SubVector_Vector3(
                        _position,
                        position
                    ),
                cal);
            }
            
            //後者->前者のベクトル
            //前者：cal.SumVector_Vector3(cal.MultipleScalar_Vector3(localX, _position[0]), cal.SumVector_Vector3(cal.MultipleScalar_Vector3(localY, _position[1]), cal.MultipleScalar_Vector3(localZ, _position[2])))
            //後者：cal.SumVector_Vector3(cal.MultipleScalar_Vector3(localX, position[0]), cal.SumVector_Vector3(cal.MultipleScalar_Vector3(localY, position[1]), cal.MultipleScalar_Vector3(localZ, position[2])))
            position[0] = _position[0];
            position[1] = _position[1];
            position[2] = _position[2];
        }
        public void AddPosition(float[] displace, Calculate cal)
        {
            changed = true;
            if (hasParent)
            {
                AddSelfAndChildrenTreeWorldPosition(cal.SumVector_Vector3(cal.MultipleScalar_Vector3(parent.localX, displace[0]), cal.SumVector_Vector3(cal.MultipleScalar_Vector3(parent.localY, displace[1]), cal.MultipleScalar_Vector3(parent.localZ, displace[2]))), cal);
            }
            else
            {
                AddSelfAndChildrenTreeWorldPosition(displace, cal);
            }
            
            position[0] += displace[0];
            position[1] += displace[1];
            position[2] += displace[2];
        }
        public void SetRotation(float[] _rotation, Calculate cal)
        {
            changed = true;
            bool[] rotationMask = [true, true, true];
            if (_rotation[2] != rotation[2])
            {
                rotationMask[2] = false;
            }
            if (_rotation[1] != rotation[1])
            {
                rotationMask[1] = false;
            }
            if (_rotation[0] != rotation[0])
            {
                rotationMask[0] = false;
            }
            if (hasParent)
            {
                RotateSelfAndChildrenTree(this.parent.worldPosition, cal.SubVector_Vector3(_rotation, rotation), rotationMask, cal);
            }
            else
            {
                RotateSelfAndChildrenTree([0, 0, 0], cal.SubVector_Vector3(_rotation, rotation), rotationMask, cal);
            }
            rotation[0] = _rotation[0] % 360;
            rotation[1] = _rotation[1] % 360;
            rotation[2] = _rotation[2] % 360;
            RecalLocalDirectionsByRotation(cal);
        }
        public void AddRotation(float[] disrotate, Calculate cal)
        {
            changed = true;
            cal.SumVector_Vector3_writeToRef(rotation, disrotate, ref rotation);
            rotation[0] %= 360;
            rotation[1] %= 360;
            rotation[2] %= 360;
            bool[] rotationMask = [true, true, true];
            if (disrotate[2] != 0)
            {
                rotationMask[2] = false;
            }
            if (disrotate[1] != 0)
            {
                rotationMask[1] = false;
            }
            if (disrotate[0] != 0)
            {
                rotationMask[0] = false;
            }
            if (hasParent)
            {
                RotateSelfAndChildrenTree(this.parent.worldPosition, disrotate, rotationMask, cal);
            }
            else
            {
                RotateSelfAndChildrenTree([0, 0, 0], disrotate, rotationMask, cal);
            }
        }

        public void EnableXShaftRotationOnLocalDirections(bool torf)
        {
            enableXShaft = torf;
        }
        public void EnableYShaftRotationOnLocalDirections(bool torf)
        {
            enableYShaft = torf;
        }
        public void EnableZShaftRotationOnLocalDirections(bool torf)
        {
            enableZShaft = torf;
        }

        //すべての子のワールド座標をずらす
        protected void AddSelfAndChildrenTreeWorldPosition(float[] displace, Calculate cal)
        {
            AddSelfWorldPosition(displace, cal);
            if (this.hasChild)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].AddSelfAndChildrenTreeWorldPosition(displace, cal);
                }
            }
        }

        protected void AddSelfWorldPosition(float[] displace, Calculate cal)
        {
            changed = true;
            cal.SumVector_Vector3_writeToRef(worldPosition, displace, ref worldPosition);
        }

        //現在のワールド座標をある地点周りに回転したワールド座標を設定する
        protected void RotateSelf(float[] _position, float[] _rotation, Calculate cal)
        {
            changed = true;
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint(_position, _rotation, worldPosition, [false, false, false]), _position, ref worldPosition);
        }

        protected void RotateSelf(float[] _position, float[] _rotation, bool[] rotationMask, Calculate cal)
        {
            changed = true;
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint(_position, _rotation, worldPosition, rotationMask), _position, ref worldPosition);
        }
        protected void RecalLocalDirectionsByRotation(Calculate cal)
        {
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0, 0, 0], rotation, [1, 0, 0], [false, false, false]), [0, 0, 0], ref localX);
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0, 0, 0], rotation, [0, 1, 0], [false, false, false]), [0, 0, 0], ref localY);
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0, 0, 0], rotation, [0, 0, 1], [false, false, false]), [0, 0, 0], ref localZ);
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0, 0, 0], rotation, [1, 0, 0], [!enableXShaft, !enableYShaft, !enableZShaft]), [0, 0, 0], ref localX_fixedShaft);
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0, 0, 0], rotation, [0, 1, 0], [!enableXShaft, !enableYShaft, !enableZShaft]), [0, 0, 0], ref localY_fixedShaft);
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0, 0, 0], rotation, [0, 0, 1], [!enableXShaft, !enableYShaft, !enableZShaft]), [0, 0, 0], ref localZ_fixedShaft);
        }
        //回転→ある中心　ある回転角　自分とすべての子孫を回転
        //あるTransform:Bが別のTransform:Aの子になるとき、まずBのワールド座標はAのそれにpositionを足したものになり、そこでAを中心とし、rotationでRotateSelfする
        //次にBの子は先ほどの操作に置けるAをBとし、Bを自分として同じことをする
        protected void RotateSelfAndChildrenTree(float[] basePosition, float[] disRotate, Calculate cal)
        {
            RotateSelf(basePosition, disRotate, cal);
            RecalLocalDirectionsByRotation(cal);

            if (hasChild)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].RotateSelfAndChildrenTree(basePosition, disRotate, cal);
                }
            }
        }
        protected void RotateSelfAndChildrenTree(float[] basePosition, float[] disRotate, bool[] rotationMask, Calculate cal)
        {
            RotateSelf(basePosition, disRotate, rotationMask, cal);
            RecalLocalDirectionsByRotation(cal);

            if (hasChild)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].RotateSelfAndChildrenTree(basePosition, disRotate, rotationMask, cal);
                }
            }
        }
        //自分以下の子孫のワールド座標を、あるTransformを親にとって再計算する
        protected void ReCal(Transform _parent, Calculate cal)
        {
            //すべての子孫のワールド座標を回転適用前の状態にする
            //このとき、x,y,zは親にとったtransformのlocalX,localY,localZで無ければならない
            //ローカル方向ベクトル
            this.localX[0] = _parent.localX[0];
            this.localX[1] = _parent.localX[1];
            this.localX[2] = _parent.localX[2];
            this.localY[0] = _parent.localY[0];
            this.localY[1] = _parent.localY[1];
            this.localY[2] = _parent.localY[2];
            this.localZ[0] = _parent.localZ[0];
            this.localZ[1] = _parent.localZ[1];
            this.localZ[2] = _parent.localZ[2];

            cal.SumVector_Vector3_writeToRef(_parent.worldPosition, cal.SumVector_Vector3(cal.MultipleScalar_Vector3(this.localX, this.position[0]), cal.SumVector_Vector3(cal.MultipleScalar_Vector3(this.localY, this.position[1]), cal.MultipleScalar_Vector3(this.localZ, this.position[2]))), ref this.worldPosition);

            if (hasChild)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].ReCal(this, cal);
                }
            }
            //末端から回転を適用していく
            RotateSelfAndChildrenTree(_parent.worldPosition, this.rotation, cal);
        }
    }
    public class ThreeDirectionalRenderingController
    {
        //ユーザーはこのクラスを介して3Dレンダリングを行う
        //アクセス自体は制限しないが、このクラスを介さず操作を行った場合、正常な動作は保障しませんよという感じ
        //2Dは仕組みが単純なのでこのようなクラスは実装しない方針
        private string guideLine =
            "3Dレンダリングを行う機構を管理する、ThreeDirectionalRenderingControllerのガイドラインです" + Environment.NewLine +
            "このクラスを介して行われた操作に対して正常な動作を保障します。" + Environment.NewLine +
            "このクラスを介さず操作を行った場合、正常な動作は保障されません。" + Environment.NewLine +
            "以下に、ユーザーが操作を行うことを想定していないクラスを列挙します。" + Environment.NewLine +
            "ThreeDirectionalCamera," + Environment.NewLine +
            "TriangleRenderingData," + Environment.NewLine +
            "CameraManager," + Environment.NewLine +
            "RendererManager" + Environment.NewLine +
            "ThreeDirectionalRenderer" + Environment.NewLine +
            "また、Polygonに所持されている三角形に対し、描画に影響する何らかの変更を行った上、その変更を反映させる場合、" + Environment.NewLine +
            "PolygonのFlagReCalc関数を実行してください。" + Environment.NewLine +
            "使用法に関する説明はGuide関数にint型の2を渡して取得してください。";
        private string usage =
            "以下に、実行可能な関数とその詳細を説明します。" + Environment.NewLine +
            "";
        private string tips =
            "一般的な順序として、" + Environment.NewLine +
            "AddRendererForScreen," + Environment.NewLine +
            "AddCamera," + Environment.NewLine +
            "SetOutputScreenForCamera," + Environment.NewLine +
            "InformPolygonToCamera" + Environment.NewLine +
            "が望ましい。" + Environment.NewLine +
            "特にユーザー側から指定することを想定している値のうち、このクラスの関数の引数にないもの" + Environment.NewLine +
            "ポリゴンの、カメラからの表示非表示" + Environment.NewLine;

        ScreenManager screenManager;
        CameraManager cameraManager = new CameraManager();
        RendererManager rendererManager;
        Calculate cal;


        /// <summary>
        /// このクラスの説明
        /// ガイドラインが必要なら1,使用法が必要なら2,両方なら0を渡す
        /// </summary>
        /// <returns></returns>
        public string Guide(int request)
        {
            if (request == 0)
            {
                return guideLine + usage;
            }
            else if (request == 1)
            {
                return guideLine;
            }
            else if (request == 2)
            {
                return usage;
            }
            else if (request == 3)
            {
                return tips;
            }
            else
            {
                return guideLine;
            }
        }
        ///Screenは自分で追加しているという前提でやるので、このクラスは
        ///スクリーンに対するレンダラーの斡旋、カメラとPolygonの連携の保障を行う
        ///ScreenManagerは前提
        ///一般的な手順：描画したいスクリーンのためのレンダラーを追加する
        ///カメラを追加する
        ///カメラの描画先を指定する
        ///ポリゴンをカメラに知らせる
        ///以上？
        public ThreeDirectionalRenderingController(ScreenManager _screenManager, Calculate _cal)
        {
            screenManager = _screenManager;
            rendererManager = new RendererManager(screenManager);
            cal = _cal;
        }
        public int AddRendererForScreen(bool _whichRotate_ray_world, int screenId, float depth, float width, float uniformedRayLength, double smallNum)
        {
            if (rendererManager.HasThisScreen(screenId))
            {
                return -1; //すでに追加されている
            }
            else
            {
                rendererManager.AddRendererForScreen(_whichRotate_ray_world, screenId, depth, width, uniformedRayLength, smallNum, cal);
                return 0;
            }
        }
        public int AddCamera(bool _whichRotate_ray_world, Mono attachingMono, char empty_letter)
        {
            if (attachingMono.HasComponent<Transform>())
            {
                ThreeDirectionalCamera tdc = new ThreeDirectionalCamera(_whichRotate_ray_world, empty_letter, screenManager, rendererManager, cal);
                attachingMono.AddComponent(tdc);

                return cameraManager.AddCamera(tdc); //idを返す(0以上)
            }
            else
            {
                return -1; //MonoにTransformがなかった
            }
        }
        public int SetOutputScreenForCamera(int screenId, int cameraId)
        {
            if (rendererManager.HasThisScreen(screenId))
            {
                if (cameraManager.HasThisCamera(cameraId))
                {
                    if (cameraManager.GetCameraById(cameraId).IsRayRotate() == rendererManager.GetRendererForThisScreen(screenId).IsRayRotate())
                    {
                        cameraManager.GetCameraById(cameraId).AddDrawingScreenList(screenId);
                    }
                    return 0;
                }
                else
                {
                    return -1; //カメラがマネージャーになかった
                }
            }
            else
            {
                return -2; //スクリーンにレンダラーがなかった
            }
        }
        public Polygon MakePolygon()
        {
            Polygon pol = new Polygon(cameraManager);
            return pol;
        }
        public int InformBoundingBoxToCamera(BoundingBox box, int cameraId)
        {
            if (cameraManager.HasThisCamera(cameraId))
            {
                cameraManager.AddBoundingBoxToCamera(cameraId, box);
                return 0;
            }
            else
            {
                return -1; //マネージャーにカメラがなかった
            }
        }
        public int InformPolygonToCamera(Polygon pol, int cameraId)
        {
            if (cameraManager.HasThisCamera(cameraId))
            {
                pol.InformMeToCamera(cameraId);
                return 0;
            }
            else
            {
                return -1; //マネージャーにカメラがなかった
            }
        }
        public int DeleteCamera(int cameraId)
        {
            if (cameraManager.HasThisCamera(cameraId))
            {
                cameraManager.DeleteCamera(cameraId);
                return 0;
            }
            else
            {
                return -1; //マネージャーにカメラがなかった
            }
        }
    }
    public class Polygon : Component
    {
        //親のTransformは監視するが、三角形は監視しないので、もし自分で三角形に変更を加えたなら主導でFlagReCalcする必要がある。
        CameraManager cameraManager;

        private Dictionary<int, int> cameraId_thisPolygonId = new Dictionary<int, int>();
        private Dictionary<int, bool> cameraId_isVisible = new Dictionary<int, bool>();

        private List<Triangle> triangles = new List<Triangle>();
        private Dictionary<int, int> id_index = new Dictionary<int, int>();

        private bool reCalcFlag;
        //Monoを監視して、再描画フラグを立てる
        private float[] lastPosition; // これはワールド座標
        private float[] lastRotation;

        private Transform parentTransform; //PolygonはTriangleのTransformをMonoのTransformの子にする

        //Triangleのid作成
        private int idGenerater = 0;

        private bool changed;

        public Polygon(CameraManager _camMn)
        {
            cameraManager = _camMn;
        }
        public override void FlameDo(float delta_time)
        {
            //今は特に特別な動作は想定していない
            for (int i = 0; i < triangles.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (triangles[i].GetVertexTransform_byIndex(j).IsChanged())
                    {
                        triangles[i].GetVertexTransform_byIndex(j).DeFlagChanged();
                        changed = true;
                    }
                }
            }
            InformRecalcIfNeeded();
        }
        public override void FlameDo()
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (triangles[i].GetVertexTransform_byIndex(j).IsChanged())
                    {
                        triangles[i].GetVertexTransform_byIndex(j).DeFlagChanged();
                        changed = true;
                    }
                }
            }
            InformRecalcIfNeeded();
        }
        public override void Attach(Mono _attachingMono)
        {
            base.Attach(_attachingMono);
            if (attachingMono.HasComponent<Transform>() == true)
            {
                parentTransform = attachingMono.GetComponent<Transform>();
                lastPosition = new float[3];
                lastRotation = new float[3];
                lastPosition[0] = parentTransform.GetWorldPosition()[0];
                lastPosition[1] = parentTransform.GetWorldPosition()[1];
                lastPosition[2] = parentTransform.GetWorldPosition()[2];
                lastRotation[0] = parentTransform.GetRotation()[0];
                lastRotation[1] = parentTransform.GetRotation()[1];
                lastRotation[2] = parentTransform.GetRotation()[2];
            }
            else
            {
                //エラー
                Console.WriteLine("error");
                Console.ReadLine();
            }
        }
        /// <summary>
        /// 三角形を追加する。
        /// return：このPolygonでの三角形のID
        /// </summary>
        /// <param name="tri"></param>
        /// <param name="cal"></param>
        public int AddTriangle(Triangle tri, Calculate cal)
        {
            //親のTransformを設定
            SetParentOfTriangleTransforms(tri.GetVertexTransforms(), cal);
            triangles.Add(tri);
            id_index.Add(idGenerater, triangles.Count - 1);
            tri.SetId(idGenerater);
            foreach (KeyValuePair<int, int> pair in cameraId_thisPolygonId)
            {
                cameraManager.InformNewTriangleInPolygonToCamera(pair.Key, pair.Value, tri);
            }
            idGenerater += 1;
            FlagReCalc();
            return idGenerater - 1;
        }
        /// <summary>
        /// IDを指定して三角形を削除する
        /// return：-1⇒IDに対応する三角形が存在しなかった
        /// </summary>
        /// <param name="triangleId"></param>
        /// <returns></returns>
        public int DeleteTriangle(int triangleId)
        {
            if (id_index.ContainsKey(triangleId))
            {
                foreach (KeyValuePair<int, int> pair in cameraId_thisPolygonId)
                {
                    cameraManager.DeleteTriangleDataFromCamera(pair.Key, pair.Value, triangleId);
                }
                triangles[id_index[triangleId]] = triangles[triangles.Count - 1];
                id_index[id_index.FirstOrDefault(kvp => kvp.Value == triangles.Count - 1).Key] = id_index[triangleId];
                triangles.RemoveAt(triangles.Count - 1);
                id_index.Remove(triangleId);
                return 0;
            }
            return -1;
        }
        /// <summary>
        /// IDを指定してカメラの描画対象にする
        /// return：-1⇒すでに描画対象だった
        /// </summary>
        /// <param name="cameraId"></param>
        public int InformMeToCamera(int cameraId)
        {
            if (cameraId_thisPolygonId.ContainsKey(cameraId))
            {
                return -1;
            }
            else
            {
                cameraId_thisPolygonId.Add(cameraId, 0);
                //ここでカメラマネージャーを介してカメラからidをもらう
                cameraManager.InformPolygonToCamera(this, cameraId);
                cameraId_isVisible.Add(cameraId, true);
                FlagReCalc();
                return 0;
            }
        }
        /// <summary>
        /// IDを指定してカメラの描画対象から外す
        /// return:-1⇒そのカメラの描画対象になっていなかった
        /// </summary>
        /// <param name="cameraId"></param>
        public int MisInformMeFromCamera(int cameraId)
        {
            if (cameraId_thisPolygonId.ContainsKey(cameraId))
            {
                cameraManager.MisInformPolygonFromCamera(cameraId, cameraId_thisPolygonId[cameraId]);
                cameraId_isVisible.Remove(cameraId);
                cameraId_thisPolygonId.Remove(cameraId);
                return 0;
            }
            return -1;
        }
        //これを毎フレーム実行する
        private void InformRecalcIfNeeded()
        {
            if (changed)
            {
                foreach (KeyValuePair<int, bool> pair in cameraId_isVisible)
                {
                    if (pair.Value)
                    {
                        cameraManager.InformToCamera_reCalcNeeded(pair.Key, cameraId_thisPolygonId[pair.Key]);
                    }
                }
            }
        }
        //カメラからidをもらうための関数
        /// <summary>
        /// !!!!!ユーザーからのアクセスを想定していない関数です!!!!!
        /// </summary>
        /// <param name="cameraId"></param>
        /// <param name="_id"></param>
        public void SetIdFromCamera(int cameraId, int _id)
        {
            cameraId_thisPolygonId[cameraId] = _id;
        }
        /// <summary>
        /// IDを指定しカメラから表示されるようにする
        /// </summary>
        /// <param name="cameraId">カメラのID</param>
        /// <returns>0:正常に完了, -1:そのカメラの描画対象になっていない</returns>
        public int EnVisibleFromCamera(int cameraId)
        {
            if (cameraId_isVisible.ContainsKey(cameraId))
            {
                cameraId_isVisible[cameraId] = true;
                return 0;
            }
            return -1;
        }
        /// <summary>
        /// IDを指定しカメラから非表示にする
        /// </summary>
        /// <param name="cameraId">カメラのID</param>
        /// <returns>0:正常に完了, -1:そのカメラの描画対象になっていない</returns>
        public int InVisibleFromCamera(int cameraId)
        {
            if (cameraId_isVisible.ContainsKey(cameraId))
            {
                cameraId_isVisible[cameraId] = false;
                return 0;
            }
            return -1;
        }
        public int SetVisibleFromCamera(int cameraId, bool isVisible)
        {
            if (cameraId_isVisible.ContainsKey(cameraId))
            {
                cameraId_isVisible[cameraId] = isVisible;
                return 0;
            }
            return -1;
        }
        /// <summary>
        /// IDを指定しカメラからの表示/非表示を返す
        /// </summary>
        /// <param name="cameraId">カメラのID</param>
        /// <returns>true:表示, false:非表示</returns>
        public bool IsVisibleFromCamera(int cameraId)
        {
            return cameraId_isVisible[cameraId];
        }
        public List<Triangle> GetTriangleList()
        {
            return triangles;
        }
        /// <summary>
        /// IDを指定して三角形への参照を得る
        /// nullの可能性がある
        /// </summary>
        /// <param name="id">三角形のID</param>
        /// <returns>IDに対応する三角形が含まれていなければnull</returns>
        public Triangle? GetTriangleById(int id)
        {
            if (id_index.ContainsKey(id))
            {
                return triangles[id_index[id]];
            }
            else
            {
                return null;
            }
        }
        public Triangle GetTriangleByIndex(int index)
        {
            return triangles[index];
        }
        /// <summary>
        /// 再計算が必要かを取得する
        /// </summary>
        /// <returns>true:必要, false:不要</returns>
        public bool GetReCalcFlag()
        {
            return reCalcFlag;
        }
        public void FlagReCalc()
        {
            reCalcFlag = true;
        }
        public void DeflagReCalc()
        {
            reCalcFlag = false;
        }
        private void SetParentOfTriangleTransforms(Transform[] transforms, Calculate cal)
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                parentTransform.AddChild(transforms[i], cal);
            }
        }
    }
    public class Triangle
    {
        //三つのTransformをもつ
        private Transform[] vertexes = new Transform[3];

        private int id;

        //描画情報を持つ
        private Texture texture;
        int mode = 0;

        bool useAsRect = false;

        public float[] forth_point = new float[3];

        float[] center = new float[3];
        float[] face_vector = new float[3];

        public Triangle(float[] _localPoint_first, float[] _localPoint_second, float[] _localPoint_third, Texture _texture, int _mode, bool _useAsRect, Calculate cal)
        {
            vertexes[0] = new Transform(_localPoint_first, [0, 0, 0], cal); //末端なので回転は意味をなさない
            vertexes[1] = new Transform(_localPoint_second, [0, 0, 0], cal);
            vertexes[2] = new Transform(_localPoint_third, [0, 0, 0], cal);
            texture = _texture;
            mode = _mode;
            useAsRect = _useAsRect;
        }
        /// <summary>
        /// !!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="_id"></param>
        public void SetId(int _id)
        {
            id = _id;
        }
        public bool IsUseAsRect()
        {
            return useAsRect;
        }
        public float[] GetForthPoint()
        {
            return forth_point;
        }
        //三点のワールド座標を取得
        public Transform[] GetVertexTransforms()
        {
            return vertexes;
        }
        public Transform GetVertexTransform_byIndex(int index)
        {
            return vertexes[index];
        }
        public Texture GetTexture()
        {
            return texture;
        }
        public int GetMode()
        {
            return mode;
        }
        public void SetTexture(Texture _texture)
        {
            texture = _texture;
        }
        public void SetMode(int _mode)
        {
            mode = _mode;
        }
    }
    public class ThreeDirectionalCamera : Component
    {
        Calculate cal;

        ScreenManager scManager;

        //描画するポリゴンの参照
        List<Polygon> drawingPolygons;
        //Polygonのほうで描画する三角形のワールド座標だけは計算してもらって、それを参照してカメラ基準座標に直す
        //ので、Polygonが自身をカメラに追加しにくる感じ

        char empty_letter;

        private int id;

        private bool isTransformChanged;
        Transform attachedMono_transform;

        //変動がすくないデータはあらかじめ取得しておいたほうが計算速度が早い。
        //数だけが合うリストを用意するという試み
        //idは引き続き使用して、この内部でidを再現する
        //ポリゴンのidと配列上でのインデックスを紐づけるリスト
        //ポリゴンのid
        List<Polygon> polygons = new List<Polygon>();

        Dictionary<int, List<int>> polygonId_polAndTrisIndexList = new Dictionary<int, List<int>>(); //allPolygons はこの辞書の追加順に並んでいるので、これで検索を行い、そのインデックス番号を取得する。


        List<int> reCalcNeededPolygonIds = new List<int>();

        private int idGenerater = 0;

        private List<int> drawingScreenId;

        //buffer
        private float[] a_vector_buffer;
        private float[] b_vector_buffer;

        private float[] firstV_buffer;
        private float[] secondV_buffer;
        private float[] thirdV_buffer;



        private Dictionary<int, int> id_boxIndexes = new Dictionary<int, int>();
        private List<BoundingBox> boxes = new List<BoundingBox>();

        //描画図形を集計して、レンダラーにscManagerの参照と一緒に渡す簡単なお仕事
        //3Dの場合
        public ThreeDirectionalCamera(char _empty_letter, ScreenManager _scMn, Calculate _cal)
        {
            scManager = _scMn;
            drawingPolygons = new List<Polygon>();
            empty_letter = _empty_letter;
            cal = _cal;
            drawingScreenId = new List<int>();
        }
        public override void FlameDo(float delta_time)
        {
        }
        public override void FlameDo()
        {
            //ここで描画したいよね
            foreach (int scrId in drawingScreenId)
            {
                if (scManager.ExistThisId(scrId))
                {
                    Draw3Dto2DScreen(scrId, cal);
                }
            }
        }
        public override void Attach(Mono _attachingMono)
        {
            base.Attach(_attachingMono);
            if (attachingMono.HasComponent<Transform>() == true)
            {
                attachedMono_transform = attachingMono.GetComponent<Transform>();
            }
            else
            {
                //エラー
            }
        }
        public void SetId(int _id)
        {
            id = _id;
        }
        public void AddNewPolygon(Polygon polygonRef)
        {
            List<int> polIndex_triDatasIndexes = new List<int>();
            polygons.Add(polygonRef);
            polIndex_triDatasIndexes.Add(polygons.Count - 1);
            List<Triangle> triList = polygonRef.GetTriangleList();
            for (int i = 0; i < triList.Count; i++)
            {
                triangleDatas.Add(new TriangleRenderingData(triList[i].GetTexture(), triList[i].GetMode()));
                polIndex_triDatasIndexes.Add(triangleDatas.Count - 1);
            }
            polygonId_polAndTrisIndexList.Add(idGenerater, polIndex_triDatasIndexes);
            polygonRef.SetIdFromCamera(this.id, idGenerater);
            idGenerater += 1;
        }
        public void RemovePolygon(int polId)
        {
            int polIndex = polygonId_polAndTrisIndexList[polId][0];
            for (int i = 0; i < polygonId_polAndTrisIndexList[polId].Count - 1; i++)
            {
                RemoveTriangle(i, polId);
            }
            polygons[polIndex] = polygons[polygons.Count - 1];
            polygonId_polAndTrisIndexList[polygonId_polAndTrisIndexList.FirstOrDefault(kvp => kvp.Value[0] == polygons.Count - 1).Key][0] = polIndex;
            polygons.RemoveAt(polygons.Count - 1);
            polygonId_polAndTrisIndexList.Remove(polId);
        }
        public void RemoveTriangle(int triIndexInPol, int polId)
        {
            triangleDatas[polygonId_polAndTrisIndexList[polId][triIndexInPol + 1]] = triangleDatas[triangleDatas.Count - 1];
            bool ended = false;
            foreach (var pair in polygonId_polAndTrisIndexList)
            {
                for (int i = 1; i < pair.Value.Count - 1; i++)
                {
                    if (pair.Value[i] == triangleDatas.Count - 1)
                    {
                        pair.Value[i] = polygonId_polAndTrisIndexList[polId][triIndexInPol + 1];
                        ended = true;
                        break;
                    }
                }
                if (ended)
                {
                    break;
                }
            }
            triangleDatas.RemoveAt(triangleDatas.Count - 1);
            polygonId_polAndTrisIndexList[polId].RemoveAt(triIndexInPol + 1);
        }
        public void AddNewTriangle(int polygonId, Triangle tri)
        {
            triangleDatas.Add(new TriangleRenderingData(tri.GetTexture(), tri.GetMode()));
            polygonId_polAndTrisIndexList[polygonId].Add(triangleDatas.Count - 1);
        }
        public void AddRecalcNeededPolygonId(int polygonId)
        {
            reCalcNeededPolygonIds.Add(polygonId);
        }
        public void AddDrawingScreenList(int screenId)
        {
            if (drawingScreenId.Contains(screenId))
            {

            }
            else
            {
                drawingScreenId.Add(screenId);
            }
        }
        public void DeleteFromDrawingScreenList(int screenId)
        {
            if (drawingScreenId.Contains(screenId))
            {
                drawingScreenId.Remove(screenId);
            }
        }
        public void Draw3Dto2DScreen(int screenId, Calculate cal)
        {
            if (whichRotate_ray_world)
            {
                rendererManager.RenderToScreen(boxes, screenId, attachedMono_transform.GetWorldPosition(), attachedMono_transform.GetRotation(), empty_letter, id);
            }
            else
            {
                CalculateRenderingTriangle(attachedMono_transform.GetWorldPosition(), attachedMono_transform.GetRotation(), cal);
                triDataBuffer.Clear();
                foreach (var pair in polygonId_polAndTrisIndexList)
                {
                    if (polygons[pair.Value[0]].IsVisibleFromCamera(this.id) != false)
                    {

                        for (int i = 1; i < pair.Value.Count; i++)
                        {
                            triDataBuffer.Add(triangleDatas[pair.Value[i]]);
                        }
                    }
                }
                rendererManager.RenderToScreen(triDataBuffer, screenId, empty_letter, id);
            }
        }

        //座標三角形をつかって描画三角形のカメラ基準座標を計算する
        public void CalculateRenderingTriangle(float[] cam_position, float[] cam_rotation, Calculate cal)
        {
            for (int i = 0; i < reCalcNeededPolygonIds.Count; i++)
            {
                for (int j = 1; j < polygonId_polAndTrisIndexList[reCalcNeededPolygonIds[i]].Count; j++)
                {
                    firstV_buffer = cal.SetPositionFromView(cam_position, cam_rotation, polygons[polygonId_polAndTrisIndexList[reCalcNeededPolygonIds[i]][0]].GetTriangleByIndex(j - 1).GetVertexTransform_byIndex(0).GetWorldPosition(), [false, false, false]);
                    secondV_buffer = cal.SetPositionFromView(cam_position, cam_rotation, polygons[polygonId_polAndTrisIndexList[reCalcNeededPolygonIds[i]][0]].GetTriangleByIndex(j - 1).GetVertexTransform_byIndex(1).GetWorldPosition(), [false, false, false]);
                    thirdV_buffer = cal.SetPositionFromView(cam_position, cam_rotation, polygons[polygonId_polAndTrisIndexList[reCalcNeededPolygonIds[i]][0]].GetTriangleByIndex(j - 1).GetVertexTransform_byIndex(2).GetWorldPosition(), [false, false, false]);
                    cal.SubVector_Vector3_writeToRef(secondV_buffer, firstV_buffer, ref a_vector_buffer);
                    cal.SubVector_Vector3_writeToRef(thirdV_buffer, firstV_buffer, ref b_vector_buffer);

                    //データを書き込む
                    triangleDatas[polygonId_polAndTrisIndexList[reCalcNeededPolygonIds[i]][j]].SetNeededDatas(a_vector_buffer, b_vector_buffer, firstV_buffer);
                    triangleDatas[polygonId_polAndTrisIndexList[reCalcNeededPolygonIds[i]][j]].ReCalcNeededProperties(cal);
                }
            }
            reCalcNeededPolygonIds.Clear();
        }
        public bool IsRayRotate()
        {
            return whichRotate_ray_world;
        }
        //バウンディングボックスを使う場合
        public int AddBoundingBox(BoundingBox box)
        {
            boxes.Add(box);
            id_boxIndexes.Add(idGenerater, boxes.Count - 1);
            idGenerater += 1;
            return idGenerater - 1;
        }
        public bool RemoveBoundingBox(int boxId)
        {
            if (id_boxIndexes.ContainsKey(boxId))
            {
                boxes[id_boxIndexes[boxId]] = boxes[boxes.Count - 1];
                id_boxIndexes[id_boxIndexes.FirstOrDefault(kvp => kvp.Value == boxes.Count - 1).Key] = id_boxIndexes[boxId];
                id_boxIndexes.Remove(boxId);
                boxes.RemoveAt(boxes.Count - 1);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class TwoDirectionalCamera
    {
        char empty_letter;

        private int id;

        ScreenManager scManager;

        float[] anchorPoint;
        float width;
        float height;
        TwoDirectionalRenderer twoDR;
        int arr_width;
        int arr_height;


        //2Dの場合
        public TwoDirectionalCamera(float[] _anchorPoint, float _width, float _height, int _arrWidth, int _arrHeight, char _empty_letter, TwoDirectionalRenderer _twoDR, ScreenManager _scMn)
        {
            twoDR = _twoDR;
            empty_letter = _empty_letter;
            scManager = _scMn;
            anchorPoint = _anchorPoint;
            width = _width;
            height = _height;
            arr_width = _arrWidth;
            arr_height = _arrHeight;
        }
        //渡されたFigure群をワールド座標のzが小さい順に並べなおしたものを渡す
        //基本的にz座標は変更しないので、あまり並べなおしはしない
        public void Draw2DAreaTo2DScreen(int screenIndex, List<Figure> drawingFigures, bool sorted, Calculate cal)
        {
            if (sorted == false)
            {
                float minZ = drawingFigures[0].GetTransform().GetWorldPosition()[2];
                Figure temp = drawingFigures[0];
                int temp_index = 0;
                for (int i = 0; i < drawingFigures.Count - 1; i++)
                {
                    for (int j = i; j < drawingFigures.Count; j++)
                    {
                        if (minZ > drawingFigures[j].GetTransform().GetWorldPosition()[2])
                        {
                            minZ = drawingFigures[j].GetTransform().GetWorldPosition()[2];
                            temp = drawingFigures[j];
                            temp_index = j;
                        }
                    }
                    if (temp_index != i)
                    {
                        drawingFigures[temp_index] = drawingFigures[i];
                        drawingFigures[i] = temp;
                    }
                }
            }
            twoDR.Render(drawingFigures, anchorPoint, width, height, arr_width, arr_height, empty_letter, screenIndex, scManager, cal);
        }


    }

    ///以下、非コンポーネントなクラス
    public class Input
    {

        public MOUSEPOINT mousePoint;
        public ConsoleKeyInfo keyInfo;

        private bool inWindow; //カーソルがウィンドウ内にあるか
        private float[] aspectMousePositionInWindow; //ウィンドウ内でのマウスの比率位置
        private float[] aspectMousePositionInScreen; //全画面内でのマウスの比率位置

        private bool[] getInputFromInfo = [false, false]; //mouse, keyの順

        private bool key_readed = false;
        private int[] mouse_move = [0, 0];
        private int[] lastMousePosition = [0, 0];

        private char keyChar;

        private uint vKeyCode;
        private int controllState;

        private bool mouse_clicked;
        private int mouse_button;

        private NativeMethods.ConsoleHandle handle;

        private int mode;

        private NativeMethods.INPUT_RECORD record;
        private uint recordLen;

        //VirtualKeyのキーコード対応
        //https://learn.microsoft.com/ja-jp/windows/win32/inputdev/virtual-key-codes

        public uint VKEY_0 = 48;
        public uint VKEY_1 = 49;
        public uint VKEY_2 = 50;
        public uint VKEY_3 = 51;
        public uint VKEY_4 = 52;
        public uint VKEY_5 = 53;
        public uint VKEY_6 = 54;
        public uint VKEY_7 = 55;
        public uint VKEY_8 = 56;
        public uint VKEY_9 = 57;
        public uint VKEY_A = 65;
        public uint VKEY_B = 66;
        public uint VKEY_C = 67;
        public uint VKEY_D = 68;
        public uint VKEY_E = 69;
        public uint VKEY_F = 70;
        public uint VKEY_G = 71;
        public uint VKEY_H = 72;
        public uint VKEY_I = 73;
        public uint VKEY_J = 74;
        public uint VKEY_K = 75;
        public uint VKEY_L = 76;
        public uint VKEY_M = 77;
        public uint VKEY_N = 78;
        public uint VKEY_O = 79;
        public uint VKEY_P = 80;
        public uint VKEY_Q = 81;
        public uint VKEY_R = 82;
        public uint VKEY_S = 83;
        public uint VKEY_T = 84;
        public uint VKEY_U = 85;
        public uint VKEY_V = 86;
        public uint VKEY_W = 87;
        public uint VKEY_X = 88;
        public uint VKEY_Y = 89;
        public uint VKEY_Z = 90;

        public uint VKEY_ESCAPE = 27;
        public uint VKEY_CONTROLL = 17;
        public uint VKEY_SHIFT = 16;
        public uint VKEY_ALT = 18;
        public uint VKEY_ENTER = 13;
        public uint VKEY_SPACE = 32;

        public uint VKEY_LEFTARROW = 37;
        public uint VKEY_UPARROW = 38;
        public uint VKEY_RIGHTARROW = 39;
        public uint VKEY_DOWNARROW = 40;

        /// <summary>
        /// コンストラクタを読んだ時点でReadKeyLoopが始まる
        /// めんどくさいので今は必ずtrue, trueにして使う
        /// </summary>
        /// <param name="fromMouse"></param>
        /// <param name="fromKeyboard"></param>
        public Input(NativeMethods.ConsoleHandle _handle)
        {
            if (true)
            {
                mousePoint = new MOUSEPOINT();
                getInputFromInfo[0] = true;
            }
            if (true)
            {
                keyInfo = new ConsoleKeyInfo();
                getInputFromInfo[1] = true;
                keyChar = '\0';
                key_readed = false;
                //Task.Run(() => ReadKeyLoop(ref keyInfo, ref key_readed));
                //本来ここでKeyをもらうタスクを走らせていたが、マウス入力と競合するのでやめた。このクラスはまだ不完全なので、この例で示した以外の使い方はおすすめできない
            }

            //正直何やってるかよくわからない
            handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, mode))) { throw new Win32Exception(); }

            record = new NativeMethods.INPUT_RECORD();
            recordLen = 0;
            Task.Run(() => ReadMouseAndKeyLoop(ref keyChar, ref key_readed, ref vKeyCode, ref controllState, ref mouse_clicked, ref mouse_button, handle, record, recordLen));
        }
        /// <summary>
        /// 入力受け取りの有効化
        /// </summary>
        /// <param name="mouseOrKeyboard"></param>
        public void EnableGettingInputFrom(int mouseOrKeyboard)
        {
            if (getInputFromInfo[mouseOrKeyboard] == true)
            {
                //すでに有効
            }
            else
            {
                if (mouseOrKeyboard == 0)
                {
                    mousePoint = new MOUSEPOINT();
                    getInputFromInfo[0] = true;
                }
                else if (mouseOrKeyboard == 1)
                {
                    keyInfo = new ConsoleKeyInfo();
                    getInputFromInfo[1] = true;
                    Task.Run(() => ReadKeyLoop(ref keyInfo, ref key_readed));
                }
            }
        }
        //Taskに渡す用の関数
        static void ReadKeyLoop(ref ConsoleKeyInfo keyInfo, ref bool key_readed)
        {
            while (true)
            {
                keyInfo = Console.ReadKey(true);
                key_readed = false;
            }
        }
        //もう一つ
        static void ReadMouseAndKeyLoop(ref char keychar, ref bool key_readed, ref uint vKeyCode, ref int cState, ref bool mouse_clicked, ref int mouse_button, NativeMethods.ConsoleHandle handle, NativeMethods.INPUT_RECORD record, uint recordLen)
        {
            while (true)
            {
                if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }
                if (record.EventType == NativeMethods.MOUSE_EVENT)
                {

                    mouse_button = record.MouseEvent.dwButtonState;
                    if (mouse_button != 0)
                    {
                        mouse_clicked = true;
                    }
                    else
                    {
                        mouse_clicked = false;
                    }
                }
                else if (record.EventType == NativeMethods.KEY_EVENT)
                {

                    key_readed = false;
                    keychar = record.KeyEvent.UnicodeChar;
                    cState = record.KeyEvent.dwControlKeyState;
                    vKeyCode = record.KeyEvent.wVirtualKeyCode;
                }
            }
        }
        /// <summary>
        /// 入力受け取りの無効化
        /// </summary>
        /// <param name="mouseOrKeyboard"></param>
        public void DisableGettingInputFrom(int mouseOrKeyboard)
        {
            if (getInputFromInfo[mouseOrKeyboard] == false)
            {
                //すでに無効
            }
            else
            {
                getInputFromInfo[mouseOrKeyboard] = false;
            }
        }
        public MOUSEPOINT GetMousePosition()
        {
            NativeMethods.GetCursorPos(out mousePoint);
            return mousePoint;
        }
        public bool IsMouseClicked()
        {
            return mouse_clicked;
        }
        public int GetMouseButton()
        {
            return mouse_button;
        }
        public char GetInputKey()
        {
            if (key_readed == false)
            {
                key_readed = true;
                return keyChar;
            }
            else
            {
                return '\0';
            }
        }
        public uint GetVirtualKeyCode()
        {
            if (key_readed == false)
            {
                key_readed = true;
                return vKeyCode;
            }
            else
            {
                return 0;
            }
        }
        public int GetControllState()
        {
            return controllState;
        }
        //Componentにすることもできるが、無駄な処理を助長する実装はせず、このような非コンポーネントだが毎フレームやる処理は、すべてEssennsials関数としてまとめて書こうと思う
        public void FlameDo()
        {
            if (getInputFromInfo[0] == true)
            {
                GetMousePosition();
                mouse_move[0] = mousePoint.x - lastMousePosition[0];
                mouse_move[1] = mousePoint.y - lastMousePosition[1];
                lastMousePosition[0] = mousePoint.x;
                lastMousePosition[1] = mousePoint.y;
            }
        }
    }
    public class GameTimer
    {
        private float delta_time = 0.0f;
        DateTime lastTime;
        DateTime currentTime;
        public GameTimer()
        {
            lastTime = DateTime.Now;
            currentTime = DateTime.Now;
        }
        public void FlameDo()
        {
            lastTime = currentTime;
            currentTime = DateTime.Now;
            delta_time = (float)(currentTime - lastTime).TotalSeconds;
        }
        public float Delta_Time()
        {
            return delta_time;
        }
    }
    public class NativeMethods
    {
        public const int HIDE = 0;
        public const int MAXIMIZE = 3;
        public const int MINIMIZE = 6;
        public const int RESTORE = 9;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetCursorPos(out MOUSEPOINT lpPoint);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ShowCursor([MarshalAs(UnmanagedType.Bool)] bool bShow);

        //コンソールウィンドウのハンドルを取得
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        //フォアグラウンドウィンドウのウィンドウハンドルを取得
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        public const int SWP_NOSIZE = 1;
        public const int SWP_NOZORDER = 4;

        //ウィンドウの領域を取得
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        //ウィンドウのクライアント領域を取得
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        //デスクトップ ウィンドウ マネージャー (DWM) 属性を取得
        //ウィンドウ可視領域を取得するために使用
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hWnd, uint dwAttribute, out RECT pvAttribute, int cbAttribute);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //マウス入力
        //https://medo64.com/posts/console-mouse-input-in-c/を参考

        public const Int32 STD_INPUT_HANDLE = -10;

        public const Int32 ENABLE_MOUSE_INPUT = 0x0010;
        public const Int32 ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const Int32 ENABLE_EXTENDED_FLAGS = 0x0080;

        public const Int32 KEY_EVENT = 1;
        public const Int32 MOUSE_EVENT = 2;


        [DebuggerDisplay("EventType: {EventType}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public Int16 EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
        }

        [DebuggerDisplay("{dwMousePosition.X}, {dwMousePosition.Y}")]
        public struct MOUSE_EVENT_RECORD
        {
            public COORD dwMousePosition;
            public Int32 dwButtonState;
            public Int32 dwControlKeyState;
            public Int32 dwEventFlags;
        }

        [DebuggerDisplay("{X}, {Y}")]
        public struct COORD
        {
            public UInt16 X;
            public UInt16 Y;
        }

        [DebuggerDisplay("KeyCode: {wVirtualKeyCode}")]
        [StructLayout(LayoutKind.Explicit)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0)]
            [MarshalAsAttribute(UnmanagedType.Bool)]
            public Boolean bKeyDown;
            [FieldOffset(4)]
            public UInt16 wRepeatCount;
            [FieldOffset(6)]
            public UInt16 wVirtualKeyCode;
            [FieldOffset(8)]
            public UInt16 wVirtualScanCode;
            [FieldOffset(10)]
            public Char UnicodeChar;
            [FieldOffset(10)]
            public Byte AsciiChar;
            [FieldOffset(12)]
            public Int32 dwControlKeyState;
        };


        public class ConsoleHandle : SafeHandleMinusOneIsInvalid
        {
            public ConsoleHandle() : base(false) { }

            protected override bool ReleaseHandle()
            {
                return true; //releasing console handle is not our business
            }
        }


        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean GetConsoleMode(ConsoleHandle hConsoleHandle, ref Int32 lpMode);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, UInt32 nLength, ref UInt32 lpNumberOfEventsRead);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean SetConsoleMode(ConsoleHandle hConsoleHandle, Int32 dwMode);
    }
    /// <summary>
    /// !!!!!ユーザーによるアクセスを想定していないクラスです!!!!!
    /// </summary>
    public class ThreeDirectionalRenderer
    {

        private float[] virtualScreen_f_vector_origin = new float[3]; 
        private float[] virtualScreen_s_vector_origin = new float[3];
        private float[] virtualScreen_t_vector_origin = new float[3];

        private float[] virtualScreen_f_vector_buffer = new float[3];
        private float[] virtualScreen_s_vector_buffer = new float[3];
        private float[] virtualScreen_t_vector_buffer = new float[3];
        private float[] virtualScreen_a_vector_buffer = new float[3];
        private float[] virtualScreen_b_vector_buffer = new float[3];


        Calculate cal;

        private char nullChar;

        
        public ThreeDirectionalRenderer(int _rayArr_width, float _depth, float _width, float _height, char _nullChar, Calculate _cal)
        {
            cal = _cal;
            nullChar = _nullChar;
        }
        public void Render(List<Triangle> tris, ScreenManager scmn, int screenId, char empty_letter, int cameraId, float[] cameraPosition, float[] cameraRotation, float depth, float width, float height)
        {
            virtualScreen_f_vector_origin = [-1 * width * 0.5f, height * 0.5f, depth];
            virtualScreen_s_vector_origin = [width * 0.5f, height * 0.5f, depth];
            virtualScreen_t_vector_origin = [-1 * width * 0.5f, -1 * height * 0.5f, depth];
            //仮想スクリーンを回転、移動
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0,0,0], cameraRotation, virtualScreen_f_vector_origin, [false, false, false]), cameraPosition, ref virtualScreen_f_vector_buffer);
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0,0,0], cameraRotation, virtualScreen_s_vector_origin, [false, false, false]), cameraPosition, ref virtualScreen_s_vector_buffer);
            cal.SumVector_Vector3_writeToRef(cal.SetPositionFromBasePoint([0, 0, 0], cameraRotation, virtualScreen_t_vector_origin, [false, false, false]), cameraPosition, ref virtualScreen_t_vector_buffer);

            cal.SubVector_Vector3_writeToRef(virtualScreen_s_vector_buffer, virtualScreen_f_vector_buffer, ref virtualScreen_a_vector_buffer);
            cal.SubVector_Vector3_writeToRef(virtualScreen_t_vector_buffer, virtualScreen_f_vector_buffer, ref virtualScreen_b_vector_buffer);

            //ここに来た時点で、
            //・陰面処理
            //・座標計算
            //・データ設定
            //・ソート
            //は済んでいる想定なので

            float DetNum = 0;
            float[] D = new float[3];
            float[] R = new float[3];
            float[] a = new float[3];
            float[] b = new float[3];
            float[] A = new float[2];

            float detA = 0;
            float detA_u = 0;
            float detA_v = 0;
            float detA_t = 0;

            bool isOut = false;

            scmn.WriteAll(screenId, empty_letter);

            int[] w_h = scmn.GetAreaInfo(screenId);
            float scrXInterval = width / (float)w_h[0];
            float scrYInterval = height / (float)w_h[1];

            int XCount = 0;
            int YCount = 0;
            int anchorIntX = 0;
            int anchorIntY = 0;
            float judger = 0;

            for (int i = 0; i < tris.Count; i++)
            {
                isOut = false;
                //遠い順に並んでいる予定
                if (tris[i].IsUseAsRect())
                {

                }
                else
                {
                    //bufferにする予定のもの
                    float minU = 2; //u,vは仮想スクリーン上での、a,bベクトル係数座標
                    float maxU = -1;
                    float minV = 2;
                    float maxV = -1;

                    float[][] tri_uv = new float[3][];

                    for (int j = 0; j < 3; j++)
                    {
                        tri_uv[j] = new float[3];
                        //一点ごとに仮想スクリーンとの交差判定を行う
                        //関数呼び出しをひどく恐れる
                        //次回、ここで仮想スクリーンとTriangleの三点のうちの一つの交差を判定し、tが正なら続行し、
                        //その三角形の三点のuv座標、そしてそこでの最小、最大のu,vを求め、
                        //仮想スクリーンにボックスが入っていれば、2Dレンダリングを参考にしてスクリーンの各点でのテクスチャを求める
                        //そして、四角形バージョンを実装し、バッファを最大限生かした仕組みにする
                        //カメラにデータ設定、陰面処理、データ設定、ソートを行う仕組みを実装
                        //強引に変更し、Polygonのみをリストとして登録可能にする
                        //コントローラーを変更する

                        float u = 0;
                        float v = 0;
                        float t = 0;
                        a = virtualScreen_a_vector_buffer;
                        b = virtualScreen_b_vector_buffer;
                        R = MultipleScalar_Vector3(SubVector_Vector3(tris[i].GetVertexTransform_byIndex(j).GetWorldPosition(), cameraPosition), -1); //三角形の頂点の座標
                        D = SubVector_Vector3(cameraPosition, virtualScreen_f_vector_buffer); //スクリーンの三角形の最初の頂点の座標
                        detA = Det(a[0], b[0], R[0], a[1], b[1], R[1], a[2], b[2], R[2]);
                        if (detA != 0)
                        {
                            detA_u = Det(D[0], b[0], R[0], D[1], b[1], R[1], D[2], b[2], R[2]);
                            detA_v = Det(a[0], D[0], R[0], a[1], D[1], R[1], a[2], D[2], R[2]);
                            detA_t = Det(a[0], b[0], D[0], a[1], b[1], D[1], a[2], b[2], D[2]);
                            //ここでテクスチャ描画に必要なu,vの値が設定される
                            u = detA_u / detA;
                            v = detA_v / detA;
                            t = detA_t / detA;

                            tri_uv[j] = [u,v,t];

                            if (minU > u)
                            {
                                minU = u;
                            }else if (maxU < u)
                            {
                                maxU = u;
                            }
                            if (minV > v)
                            {
                                minV = v;
                            }else if (maxV < v)
                            {
                                maxV = v;
                            }
                        }
                        else
                        {
                            //描画しない
                            isOut = true;
                            break;
                        }
                    }
                    if (minU <= 1 && minV <= 1 && maxU >= 0 && maxV >= 0)
                    {

                    }
                    else
                    {
                        isOut = true;
                    }
                    if (tri_uv[0][2] < 0 && tri_uv[1][2] < 0 && tri_uv[2][2] < 0)
                    {
                        isOut = true;
                    }
                    if (!RectIsInRange(minU, maxU, minV, maxV, [0,0], 1, 1))
                    {
                        isOut = true;
                    }
                    if (!isOut)
                    {
                        //描画する
                        XCount = (int)Math.Floor((float)Math.Abs(maxU - minU) / scrXInterval) + 10;
                        YCount = (int)Math.Floor((float)Math.Abs(maxV - minV) / scrYInterval) + 10;
                        anchorIntX = (int)Math.Floor(minU / scrXInterval);
                        anchorIntY = (int)Math.Floor(minV / scrYInterval);
                        cal.SubVector_Vector2_writeToRef(tri_uv[1], tri_uv[0], ref a);
                        cal.SubVector_Vector2_writeToRef(tri_uv[2], tri_uv[0], ref b);
                        DetNum = cal.Det_2(a[0], b[0], a[1], b[1]);
                        if (DetNum != 0)
                        {
                            for (int j = -10; j < YCount; j++)
                            {
                                for (int k = -10; k < XCount; k++)
                                {
                                    A = [k * scrXInterval - (tri_uv[0][0] - minU), j * scrYInterval - (tri_uv[0][1] - minV)];
                                    scmn.WritePoint_safe(screenId, [anchorIntX + k, anchorIntY + j], tris[i].GetTexture().GetLetterByXY(tris[i].GetMode(), cal.Det_2(A[0], b[0], A[1], b[1]) / DetNum, cal.Det_2(a[0], A[0], a[1], A[1]) / DetNum, empty_letter));
                                }
                            }
                        }
                    }
                }
            }
            
        }
        public bool RectIsInRange(float minU, float maxU, float minV, float maxV, float[] anchorPoint, float width, float height)
        {
            if (PointIsInRange([minU, minV], anchorPoint, width, height) || PointIsInRange([minU, maxV], anchorPoint, width, height) || PointIsInRange([maxU, minV], anchorPoint, width, height) || PointIsInRange([maxU, maxV], anchorPoint, width, height))
            {
                return true;
            }
            else
            {
                if (minU <= anchorPoint[0] && maxU >= anchorPoint[0] + width)
                {
                    if ((minV > anchorPoint[1] && minV < anchorPoint[1] + height) || (maxV > anchorPoint[1] && maxV < anchorPoint[1] + height))
                    {
                        return true;
                    }
                }
                if (minV <= anchorPoint[1] && maxV >= anchorPoint[1] + height)
                {
                    if ((minU > anchorPoint[0] && minU < anchorPoint[0] + width) || (maxU > anchorPoint[0] && maxU < anchorPoint[0] + width))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool PointIsInRange(float[] point, float[] anchorPoint, float width, float height)
        {
            if (anchorPoint[0] <= point[0] && point[0] <= anchorPoint[0] + width && anchorPoint[1] <= point[1] && point[1] <= anchorPoint[1] + height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //計算もここで行う
        //行列の値
        private float Det(float a, float b, float c, float d, float e, float f, float g, float h, float i)
        {
            return a * e * i + b * f * g + c * d * h - a * f * h - b * d * i - c * e * g;
        }
        private float[] SumVector_Vector3(float[] a, float[] b)
        {
            return [a[0] + b[0], a[1] + b[1], a[2] + b[2]];
        }
        //b -> a
        private float[] SubVector_Vector3(float[] a, float[] b)
        {
            return [a[0] - b[0], a[1] - b[1], a[2] - b[2]];
        }
        //内積
        private float[] InnerProduct_Vector3(float[] a, float[] b)
        {
            return [a[0] * b[0], a[1] * b[1], a[2] * b[2]];
        }
        private float[] MultipleScalar_Vector3(float[] a, float b)
        {
            return [a[0] * b, a[1] * b, a[2] * b];
        }
    }

    public class TwoDirectionalRenderer
    {
        //2Dゲームで、選んだ領域内でのキャラクターの存在位置を把握し、適切に描画する
        //Z軸正から負をみるようにレンダリングする
        //Threeはカメラに依存していたが、Twoは依存性が低いので、複数のカメラで併用できる。

        private float buffer_minX;
        private float buffer_maxX;
        private float buffer_minY;
        private float buffer_maxY;
        private float[][] buffer_points = new float[4][];

        private float[] A = new float[2];
        private float[] a = new float[3];
        private float[] b = new float[3];
        private float D = 0;

        private int bufferXCount = 0;
        private int bufferYCount = 0;

        private int figAnchorIntX = 0;
        private int figAnchorIntY = 0;

        private char nullChar;



        public TwoDirectionalRenderer(char _nullChar)
        {
            buffer_points[0] = new float[3];
            buffer_points[1] = new float[3];
            buffer_points[2] = new float[3];
            buffer_points[3] = new float[3];
            nullChar = _nullChar;
        }
        public void Render(List<Figure> figs, float[] anchorPoint, float width, float height, int rayArr_width, int rayArr_height, char empty_letter, int scrIndex, ScreenManager scMn, Calculate cal)
        {
            if (empty_letter != nullChar)
            {
                char letterBuffer = '\0';
                scMn.WriteAll(scrIndex, empty_letter);
                for (int i = 0; i < figs.Count; i++)
                {
                    if (figs[i].IsVisible() && FigureIsInRange(figs[i], anchorPoint, width, height, cal))
                    {
                        //この時点で、buffer_などに適切な値が格納されているので
                        //anchorPointを[0,0]とする
                        buffer_minX -= anchorPoint[0];
                        buffer_maxX -= anchorPoint[0];
                        buffer_minY -= anchorPoint[1];
                        buffer_maxY -= anchorPoint[1];
                        buffer_points[1][0] = figs[i].GetTransform().GetWorldPosition()[0] - anchorPoint[0];
                        buffer_points[1][1] = figs[i].GetTransform().GetWorldPosition()[1] - anchorPoint[1];

                        //スクリーンの分割を取得
                        buffer_points[0][0] = width / (float)rayArr_width;
                        buffer_points[0][1] = height / (float)rayArr_height;
                        //ある程度余裕をもって描画判定をしないと、角が丸まったりする
                        bufferXCount = (int)Math.Floor((float)Math.Abs(buffer_maxX - buffer_minX) / buffer_points[0][0]) + 10;
                        bufferYCount = (int)Math.Floor((float)Math.Abs(buffer_maxY - buffer_minY) / buffer_points[0][1]) + 10;
                        figAnchorIntX = (int)Math.Floor(buffer_minX / buffer_points[0][0]);
                        figAnchorIntY = (int)Math.Floor(buffer_minY / buffer_points[0][1]);
                        D = cal.Det_2(a[0], b[0], a[1], b[1]);
                        //ここにも余裕を持たせる
                        for (int j = -10; j < bufferYCount; j++)
                        {
                            for (int k = -10; k < bufferXCount; k++)
                            {
                                if (D != 0)
                                {
                                    A = [k * buffer_points[0][0] - (buffer_points[1][0] - buffer_minX), j * buffer_points[0][1] - (buffer_points[1][1] - buffer_minY)];
                                    letterBuffer = figs[i].GetTexture().GetLetterByXY(figs[i].GetMode(), cal.Det_2(A[0], b[0], A[1], b[1]) / D, cal.Det_2(a[0], A[0], a[1], A[1]) / D, empty_letter);
                                    if (letterBuffer != empty_letter)
                                    {
                                        scMn.WritePoint_safe(scrIndex, [figAnchorIntX + k, figAnchorIntY + j], letterBuffer);
                                    }
                                }
                                else
                                {
                                    scMn.WritePoint_safe(scrIndex, [figAnchorIntX + k, figAnchorIntY + j], empty_letter);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                scMn.WriteAll(scrIndex, empty_letter);
                for (int i = 0; i < figs.Count; i++)
                {
                    if (figs[i].IsVisible() && FigureIsInRange(figs[i], anchorPoint, width, height, cal))
                    {
                        //この時点で、buffer_などに適切な値が格納されているので
                        //anchorPointを[0,0]とする
                        buffer_minX -= anchorPoint[0];
                        buffer_maxX -= anchorPoint[0];
                        buffer_minY -= anchorPoint[1];
                        buffer_maxY -= anchorPoint[1];
                        buffer_points[1][0] = figs[i].GetTransform().GetWorldPosition()[0] - anchorPoint[0];
                        buffer_points[1][1] = figs[i].GetTransform().GetWorldPosition()[1] - anchorPoint[1];

                        //スクリーンの分割を取得
                        buffer_points[0][0] = width / (float)rayArr_width;
                        buffer_points[0][1] = height / (float)rayArr_height;
                        //ある程度余裕をもって描画判定をしないと、角が丸まったりする
                        bufferXCount = (int)Math.Floor((float)Math.Abs(buffer_maxX - buffer_minX) / buffer_points[0][0]) + 10;
                        bufferYCount = (int)Math.Floor((float)Math.Abs(buffer_maxY - buffer_minY) / buffer_points[0][1]) + 10;
                        figAnchorIntX = (int)Math.Floor(buffer_minX / buffer_points[0][0]);
                        figAnchorIntY = (int)Math.Floor(buffer_minY / buffer_points[0][1]);
                        D = cal.Det_2(a[0], b[0], a[1], b[1]);
                        //ここにも余裕を持たせる
                        for (int j = -10; j < bufferYCount; j++)
                        {
                            for (int k = -10; k < bufferXCount; k++)
                            {
                                if (D != 0)
                                {
                                    A = [k * buffer_points[0][0] - (buffer_points[1][0] - buffer_minX), j * buffer_points[0][1] - (buffer_points[1][1] - buffer_minY)];
                                    scMn.WritePoint_safe(scrIndex, [figAnchorIntX + k, figAnchorIntY + j], figs[i].GetTexture().GetLetterByXY(figs[i].GetMode(), cal.Det_2(A[0], b[0], A[1], b[1]) / D, cal.Det_2(a[0], A[0], a[1], A[1]) / D, empty_letter));
                                }
                            }
                        }
                    }
                }
            }

        }
        public bool FigureIsInRange(Figure fig, float[] anchorPoint, float width, float height, Calculate cal)
        {
            //参照渡しに気をつけろ！
            buffer_points[0][0] = fig.GetTransform().GetWorldPosition()[0];
            buffer_points[0][1] = fig.GetTransform().GetWorldPosition()[1];
            buffer_points[0][2] = fig.GetTransform().GetWorldPosition()[2];
            cal.SumVector_Vector3_writeToRef(buffer_points[0], cal.MultipleScalar_Vector3(fig.GetTransform().GetShaftFixedLocalXDirection(), fig.GetWidth()), ref buffer_points[1]);
            cal.SumVector_Vector3_writeToRef(buffer_points[0], cal.MultipleScalar_Vector3(fig.GetTransform().GetShaftFixedLocalYDirection(), fig.GetHeight()), ref buffer_points[2]);
            cal.SumVector_Vector3_writeToRef(buffer_points[1], cal.MultipleScalar_Vector3(fig.GetTransform().GetShaftFixedLocalYDirection(), fig.GetHeight()), ref buffer_points[3]);
            buffer_minX = buffer_points[0][0];
            buffer_maxX = buffer_points[0][0];
            buffer_minY = buffer_points[0][1];
            buffer_maxY = buffer_points[0][1];

            for (int i = 0; i < buffer_points.Length; i++)
            {
                if (buffer_minX > buffer_points[i][0])
                {
                    buffer_minX = buffer_points[i][0];
                }
                if (buffer_maxX < buffer_points[i][0])
                {
                    buffer_maxX = buffer_points[i][0];
                }
                if (buffer_minY > buffer_points[i][1])
                {
                    buffer_minY = buffer_points[i][1];
                }
                if (buffer_maxY < buffer_points[i][1])
                {
                    buffer_maxY = buffer_points[i][1];
                }
            }

            cal.SubVector_Vector3_writeToRef(buffer_points[1], buffer_points[0], ref a);
            cal.SubVector_Vector3_writeToRef(buffer_points[2], buffer_points[0], ref b);

            if (PointIsInRange([buffer_minX, buffer_minY], anchorPoint, width, height) || PointIsInRange([buffer_minX, buffer_maxY], anchorPoint, width, height) || PointIsInRange([buffer_maxX, buffer_minY], anchorPoint, width, height) || PointIsInRange([buffer_maxX, buffer_maxY], anchorPoint, width, height))
            {
                return true;
            }
            else
            {
                if (buffer_minX <= anchorPoint[0] && buffer_maxX >= anchorPoint[0] + width)
                {
                    if ((buffer_minY > anchorPoint[1] && buffer_minY < anchorPoint[1] + height) || (buffer_maxY > anchorPoint[1] && buffer_maxY < anchorPoint[1] + height))
                    {
                        return true;
                    }
                }
                if (buffer_minY <= anchorPoint[1] && buffer_maxY >= anchorPoint[1] + height)
                {
                    if ((buffer_minX > anchorPoint[0] && buffer_minX < anchorPoint[0] + width) || (buffer_maxX > anchorPoint[0] && buffer_maxX < anchorPoint[0] + width))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool PointIsInRange(float[] point, float[] anchorPoint, float width, float height)
        {
            if (anchorPoint[0] <= point[0] && point[0] <= anchorPoint[0] + width && anchorPoint[1] <= point[1] && point[1] <= anchorPoint[1] + height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    //二次元図形
    public class Figure : Component
    {
        //テクスチャとモードとTransformを持つ
        //回転をテクスチャにも適用したいので、レンダリングするときにいろいろ計算する
        Texture texture;
        int mode;

        //width, heightがこの図形の大きさを決める
        float width;
        float height;

        bool isVisible = true;

        //2Dシーンでは、左上原点のxy座標をとる
        //なので、Transformの回転は基本的にz軸回転のみとなる(他のもできるけど、ローカルのやつが軸固定されるので他軸で回転しても無意味)
        Transform transform; //アタッチ先のTransform
        public Figure(Texture _texture, int _mode, float _width, float _height)
        {
            texture = _texture;
            mode = _mode;
            width = _width;
            height = _height;
        }
        public override void Attach(Mono _attachingMono)
        {
            base.Attach(_attachingMono);
            if (attachingMono.HasComponent<Transform>() == true)
            {
                transform = attachingMono.GetComponent<Transform>();
                transform.EnableXShaftRotationOnLocalDirections(false);
                transform.EnableYShaftRotationOnLocalDirections(false);
            }
            else
            {
                //エラー
            }
        }
        public Transform GetTransform()
        {
            return transform;
        }
        public Texture GetTexture() { return texture; }
        public int GetMode() { return mode; }
        public float GetWidth() { return width; }
        public float GetHeight() { return height; }
        public void SetVisible(bool _isVisible)
        {
            isVisible = _isVisible;
        }
        public bool IsVisible()
        {
            return isVisible;
        }
        public void SetWidth(float _width)
        {
            width = _width;
        }
        public void SetHeight(float _height)
        {
            height = _height;
        }
    }

    //これ自体が何かやるわけではないのでComponentではない
    //四角形のテクスチャを用意する
    //四隅を基準としてベクトル長さから文字を渡す
    public class Texture
    {
        private int width; //データ配列の横幅
        private int height; //縦幅
        private char[] data;

        private bool repeatMode = false;
        public Texture(int width, int height, char empty_letter)
        {
            this.width = width;
            this.height = height;
            data = new char[width * height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = empty_letter;
            }
        }
        public Texture(int width, int height, char[] data)
        {
            this.width = width;
            this.height = height;
            this.data = data;
        }
        public Texture(int width, int height, char[] data, bool isRepeat)
        {
            this.width = width;
            this.height = height;
            this.data = data;
            this.repeatMode = isRepeat;
        }
        public void SetData(char[] _data)
        {
            if (_data.Length == width * height)
            {
                data = _data;
            }
        }
        public void WritePoint_safe(int[] point, char letter)
        {
            if (0 <= point[0] && point[0] < width && 0 <= point[1] && point[1] < height)
            {
                data[point[1] * width + point[0]] = letter;
            }
        }
        public char GetLetterOnPoint(int[] point, char empty_letter)
        {
            if (repeatMode)
            {
                point[0] %= width;
                point[1] %= height;
                if (point[0] < 0 || point[0] >= width || point[1] < 0 || point[1] >= height)
                {
                    return empty_letter;
                }
                else
                {
                    return data[point[1] * width + point[0]];
                }
            }
            else
            {
                if (point[0] < 0 || point[0] >= width || point[1] < 0 || point[1] >= height)
                {
                    return empty_letter;
                }
                else
                {
                    return data[point[1] * width + point[0]];
                }
            }
        }
        //modeは0~3の整数 x,yベクトルの位置と向きが変わる 正方形に対応する二次元配列を想像したとき、
        // 0: x 左上->右上, y 左上->左下
        // 1: x 左下->右下, y 左下->左上
        // 2: x 右下->左下, y 右下->右上
        // 3: x 右上->左上, y 右上->右下
        //に対応する
        //xyは理想的には0~1までの数
        public char GetLetterByXY(int mode, float x, float y, char empty_letter)
        {
            float[] buffer = new float[2];
            buffer[0] = x;
            buffer[1] = y;
            if (mode == 0)
            {
                //無変更
            }
            else if (mode == 1)
            {
                buffer[1] = 1f - buffer[1];
            }
            else if (mode == 2)
            {
                buffer[0] = 1f - buffer[0];
                buffer[1] = 1f - buffer[1];
            }
            else if (mode == 3)
            {
                buffer[0] = 1f - buffer[0];
            }
            else
            {
                //mode:0と同じ
            }

            buffer[0] *= (width);
            buffer[1] *= (height);
            return GetLetterOnPoint([(int)Math.Floor(buffer[0]), (int)Math.Floor(buffer[1])], empty_letter);

            //return GetLetterOnPoint([(int)Math.Round(buffer[0]), (int)Math.Round(buffer[1])], empty_letter);
            //return GetLetterOnPoint([(int)Math.Round(buffer[0], MidpointRounding.AwayFromZero), (int)Math.Round(buffer[1], MidpointRounding.AwayFromZero)], empty_letter);
        }
    }

    //実質的に描画を管理するクラス
    public class ScreenManager
    {
        private Dictionary<int, ScreenArea> id_area = new Dictionary<int, ScreenArea>();
        private int screen_width;
        private int screen_height;

        private ScreenArea primary_screen;

        private char nullChar;

        private char[] drawingBufferArray;

        private int idGenerator = 0;

        private ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        /*
            0:Black
            1:DarkBlue
            2:DarkGreen
            3:DarkCyan
            4:DarkRed
            5:DarkMagenta
            6:DarkYellow
            7:Gray
            8:DarkGray
            9:Blue
            10:Green
            11:Cyan
            12:Red
            13:Magenta
            14:Yellow
            15:White
        */

        //primary screenを作る
        //これはareasに含まれる文字データを書き込むバッファのようなもので、Consoleへ出力されるのはここに書き込まれた文字データ。
        //empty_letterは余白を埋める文字で、nullCharは文字が存在しないという意味の値
        //nullCharには\0を割り当てているが、これをprimary_screenに書き込んだ状態で出力すると、nullChar部分が詰められてしまうので必ず回避する(nullChar以外の文字で埋めて初期化する)
        public ScreenManager(char _nullChar, int width, int height, char empty_letter, int letterColor, int backgroundColor)
        {
            nullChar = _nullChar;
            screen_height = height;
            screen_width = width;
            drawingBufferArray = new char[width];
            Console.SetWindowSize(screen_width, screen_height);
            primary_screen = new ScreenArea(screen_width, screen_height, [0, 0], -1, empty_letter, letterColor, backgroundColor);
        }
        //不可能なら-1を返すそれ以外ならindex
        //背景を透明化したいならempty_letterをnullCharにすればいい
        public int RequestAddScreen(int[] anchor, int[] width_height, char empty_letter)
        {
            if (screen_width >= anchor[0] + width_height[0] && screen_height >= anchor[1] + width_height[1])
            {
                ScreenArea newArea = new ScreenArea(width_height[0], width_height[1], anchor, idGenerator, empty_letter, 15, 0);
                id_area.Add(idGenerator, newArea);
                idGenerator += 1;
                return idGenerator - 1;
            }
            return -1;
        }
        public int RequestAddScreen(int[] anchor, int[] width_height, char empty_letter, int letterColor, int backgroundColor)
        {
            if (screen_width >= anchor[0] + width_height[0] && screen_height >= anchor[1] + width_height[1])
            {
                ScreenArea newArea = new ScreenArea(width_height[0], width_height[1], anchor, idGenerator, empty_letter, letterColor, backgroundColor);
                id_area.Add(idGenerator, newArea);
                idGenerator += 1;
                return idGenerator - 1;
            }
            return -1;
        }
        public bool ExistThisId(int id)
        {
            if (id_area.ContainsKey(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChangeScreenColor(int letterColor, int backgroundColor)
        {
            if (0 <= letterColor && letterColor <= 15 && 0 <= backgroundColor && backgroundColor <= 15)
            {
                primary_screen.letterAndBackColor[0] = letterColor;
                primary_screen.letterAndBackColor[1] = backgroundColor;
            }
        }
        public void DrawScreen(int[] areaIds)
        {
            //areaのidを描画したい順に渡す
            for (int i = 0; i < areaIds.Length; i++)
            {
                for (int j = 0; j < id_area[areaIds[i]].height; j++)
                {
                    for (int k = 0; k < id_area[areaIds[i]].width; k++)
                    {
                        //null文字なら描画しないという方針
                        if (id_area[areaIds[i]].screenString[j * id_area[areaIds[i]].width + k] != nullChar)
                        {

                            WritePoint_safe_primaryScreen([id_area[areaIds[i]].anchor[0] + k, id_area[areaIds[i]].anchor[1] + j], id_area[areaIds[i]].screenString[j * id_area[areaIds[i]].width + k]);
                        }
                    }
                }
            }
            Console.ForegroundColor = colors[primary_screen.letterAndBackColor[0]];
            Console.BackgroundColor = colors[primary_screen.letterAndBackColor[1]];
            for (int i = 0; i < primary_screen.height; i++)
            {
                Console.SetCursorPosition(0, i);
                Array.Copy(primary_screen.screenString, i * primary_screen.width, drawingBufferArray, 0, primary_screen.width);
                Console.Write(new String(drawingBufferArray));
            }
            for (int i = 0; i < primary_screen.screenString.Length; i++)
            {
                //一応初期化しておく
                primary_screen.screenString[i] = primary_screen.empty_letter;
            }

            //色彩のareaごとの変更は実行時間の都合でできなくなった
            /*
            if (areas[index].hasNoColorChange == true)
            {
                string buffer = areas[index].screenString.ToString();
                Console.ForegroundColor = colors[areas[index].letterAndBackColor[0]];
                Console.BackgroundColor = colors[areas[index].letterAndBackColor[1]];
                for (int i = 0; i < areas[index].height; i++)
                {
                    Console.SetCursorPosition(areas[index].anchor[0], areas[index].anchor[1] + i);
                    Console.Write(buffer.Substring(areas[index].width * i, areas[index].width), areas[index].width);
                }
            }
            else
            {
                //一文字ずつ出力することになるので非常に遅い。使わない。
                string buffer = areas[index].screenString.ToString();
                for (int i = 0; i < areas[index].height; i++)
                {
                    for (int j = 0; j < areas[index].width; j++)
                    {
                        Console.SetCursorPosition(areas[index].anchor[0] + j, areas[index].anchor[1] + i);
                        Console.ForegroundColor = colors[areas[index].letterColorScreen[i * areas[index].width + j]];
                        Console.BackgroundColor = colors[areas[index].backgroundColorScreen[i * areas[index].width + j]];
                        Console.Write(buffer[i * areas[index].width + j].ToString(), 1);
                    }
                }
            }
            */
        }
        public void DrawScreen(int[] areaIds, int[] coloringAreaIndexInTheAreaIds)
        {
            //areaのidを描画したい順に渡す
            for (int i = 0; i < areaIds.Length; i++)
            {
                for (int j = 0; j < id_area[areaIds[i]].height; j++)
                {
                    for (int k = 0; k < id_area[areaIds[i]].width; k++)
                    {
                        //null文字なら描画しないという方針
                        if (id_area[areaIds[i]].screenString[j * id_area[areaIds[i]].width + k] != nullChar)
                        {

                            WritePoint_safe_primaryScreen([id_area[areaIds[i]].anchor[0] + k, id_area[areaIds[i]].anchor[1] + j], id_area[areaIds[i]].screenString[j * id_area[areaIds[i]].width + k]);
                        }
                    }
                }
            }
            int[] basePoint = [0, 0, 0, 0, 0];
            for (int i = 0; i < coloringAreaIndexInTheAreaIds.Length; i++)
            {
                char[] bufferArray = id_area[areaIds[coloringAreaIndexInTheAreaIds[i]]].bufferArray;
                Console.ForegroundColor = colors[id_area[areaIds[coloringAreaIndexInTheAreaIds[i]]].letterAndBackColor[0]];
                Console.BackgroundColor = colors[id_area[areaIds[coloringAreaIndexInTheAreaIds[i]]].letterAndBackColor[1]];
                basePoint[0] = id_area[areaIds[coloringAreaIndexInTheAreaIds[i]]].anchor[0];
                basePoint[1] = id_area[areaIds[coloringAreaIndexInTheAreaIds[i]]].anchor[1];
                basePoint[2] = basePoint[1] * primary_screen.width + basePoint[0];
                basePoint[3] = id_area[areaIds[coloringAreaIndexInTheAreaIds[i]]].width;
                basePoint[4] = id_area[areaIds[coloringAreaIndexInTheAreaIds[i]]].height;
                for (int j = 0; j < basePoint[4]; j++)
                {
                    Console.SetCursorPosition(basePoint[0], basePoint[1] + j);
                    Array.Copy(primary_screen.screenString, basePoint[2] + j * primary_screen.width, bufferArray, 0, basePoint[3]);
                    Console.Write(new String(bufferArray));
                }
            }
            for (int i = 0; i < primary_screen.screenString.Length; i++)
            {
                //一応初期化しておく
                primary_screen.screenString[i] = primary_screen.empty_letter;
            }

            //色彩のareaごとの変更は実行時間の都合でできなくなった
            /*
            if (areas[index].hasNoColorChange == true)
            {
                string buffer = areas[index].screenString.ToString();
                Console.ForegroundColor = colors[areas[index].letterAndBackColor[0]];
                Console.BackgroundColor = colors[areas[index].letterAndBackColor[1]];
                for (int i = 0; i < areas[index].height; i++)
                {
                    Console.SetCursorPosition(areas[index].anchor[0], areas[index].anchor[1] + i);
                    Console.Write(buffer.Substring(areas[index].width * i, areas[index].width), areas[index].width);
                }
            }
            else
            {
                //一文字ずつ出力することになるので非常に遅い。使わない。
                string buffer = areas[index].screenString.ToString();
                for (int i = 0; i < areas[index].height; i++)
                {
                    for (int j = 0; j < areas[index].width; j++)
                    {
                        Console.SetCursorPosition(areas[index].anchor[0] + j, areas[index].anchor[1] + i);
                        Console.ForegroundColor = colors[areas[index].letterColorScreen[i * areas[index].width + j]];
                        Console.BackgroundColor = colors[areas[index].backgroundColorScreen[i * areas[index].width + j]];
                        Console.Write(buffer[i * areas[index].width + j].ToString(), 1);
                    }
                }
            }
            */
        }

        public bool WritePoint_safe(int areaId, int[] point, char letter)
        {
            if (0 <= point[0] && point[0] < id_area[areaId].width && 0 <= point[1] && point[1] < id_area[areaId].height)
            {
                if (letter != nullChar)
                {
                    id_area[areaId].screenString[point[1] * id_area[areaId].width + point[0]] = letter;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool WritePoint_safe_primaryScreen(int[] point, char letter)
        {
            if (0 <= point[0] && point[0] < primary_screen.width && 0 <= point[1] && point[1] < primary_screen.width)
            {
                primary_screen.screenString[point[1] * primary_screen.width + point[0]] = letter;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void WritePoint(int areaId, int[] point, char letter)
        {
            id_area[areaId].screenString[point[1] * id_area[areaId].width + point[0]] = letter;
        }
        public void WritePoint(int areaId, int[] point, char letter, int letterColor)
        {
            id_area[areaId].screenString[point[1] * id_area[areaId].width + point[0]] = letter;
            if (id_area[areaId].letterColorScreen[point[1] * id_area[areaId].width + point[0]] != letterColor)
            {
                id_area[areaId].letterColorScreen[point[1] * id_area[areaId].width + point[0]] = letterColor;
                id_area[areaId].hasNoColorChange = false;
            }
        }
        public void WritePoint(int areaId, int[] point, char letter, int letterColor, int backgroundColor)
        {
            id_area[areaId].screenString[point[1] * id_area[areaId].width + point[0]] = letter;
            if (id_area[areaId].letterColorScreen[point[1] * id_area[areaId].width + point[0]] != letterColor || id_area[areaId].backgroundColorScreen[point[1] * id_area[areaId].width + point[0]] != backgroundColor)
            {
                id_area[areaId].letterColorScreen[point[1] * id_area[areaId].width + point[0]] = letterColor;
                id_area[areaId].backgroundColorScreen[point[1] * id_area[areaId].width + point[0]] = backgroundColor;
                id_area[areaId].hasNoColorChange = false;
            }
        }
        public bool WritePoint_upSetY_safe(int[] point, int areaId, char letter)
        {
            if (0 <= point[0] && point[0] < id_area[areaId].width && 0 <= point[1] && point[1] < id_area[areaId].width)
            {
                id_area[areaId].screenString[(id_area[areaId].height - 1 - point[1]) * id_area[areaId].width + point[0]] = letter;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void WritePoint_upSetY(int[] point, int areaId, char letter)
        {
            id_area[areaId].screenString[(id_area[areaId].height - 1 - point[1]) * id_area[areaId].width + point[0]] = letter;
        }
        public void WritePoint_upSetY(int[] point, int areaId, char letter, int letterColor, int backgroundColor)
        {
            id_area[areaId].screenString[(id_area[areaId].height - 1 - point[1]) * id_area[areaId].width + point[0]] = letter;
            if (id_area[areaId].letterColorScreen[(id_area[areaId].height - 1 - point[1]) * id_area[areaId].width + point[0]] != letterColor || id_area[areaId].backgroundColorScreen[(id_area[areaId].height - 1 - point[1]) * id_area[areaId].width + point[0]] != backgroundColor)
            {
                id_area[areaId].letterColorScreen[point[1] * id_area[areaId].width + point[0]] = letterColor;
                id_area[areaId].backgroundColorScreen[point[1] * id_area[areaId].width + point[0]] = backgroundColor;
                id_area[areaId].hasNoColorChange = false;
            }
        }
        public void WriteAll(int areaId, char letter)
        {
            for (int i = 0; i < id_area[areaId].screenString.Length; i++)
            {
                id_area[areaId].screenString[i] = letter;
            }
        }
        public int[] GetAreaInfo(int areaId)
        {
            return [id_area[areaId].width, id_area[areaId].height];
        }
        public char GetLetterOnPoint(int areaId, int[] point)
        {
            return id_area[areaId].screenString[(screen_height - 1 - point[1]) * id_area[areaId].width + point[0]];
        }
        public char? GetLetterOnPoint_safe(int areaId, int[] point)
        {
            if (0 <= point[0] && point[0] <= screen_width - 1 && 0 <= point[1] && point[1] <= screen_height - 1)
            {
                return GetLetterOnPoint(areaId, point);
            }
            else
            {
                return null;
            }
        }
        public bool[] GetLetterExistionAroundPoint(int areaId, int[] point, char letter)
        {
            bool[] result = new bool[4];
            if (GetLetterOnPoint_safe(areaId, [point[0], point[1] + 1]) == letter)
            {
                result[0] = true;
            }
            if (GetLetterOnPoint_safe(areaId, [point[0] + 1, point[1]]) == letter)
            {
                result[1] = true;
            }
            if (GetLetterOnPoint_safe(areaId, [point[0], point[1] - 1]) == letter)
            {
                result[2] = true;
            }
            if (GetLetterOnPoint_safe(areaId, [point[0] - 1, point[1]]) == letter)
            {
                result[3] = true;
            }
            return result;
        }
    }

    public class ScreenArea
    {
        public int width;
        public int height;
        public int[] anchor;
        public char[] screenString;
        public int[] letterColorScreen;
        public int[] backgroundColorScreen;
        public int[] letterAndBackColor = [];
        public int index;
        public bool hasNoColorChange;
        public char empty_letter;
        public char[] bufferArray;
        public ScreenArea(int _width, int _height, int[] _anchor, int _index, char _empty_letter, int letterColor, int backgroundColor)
        {
            width = _width;
            height = _height;
            anchor = _anchor;
            index = _index;
            screenString = new char[width * height];
            letterColorScreen = new int[width * height];
            backgroundColorScreen = new int[width * height];
            empty_letter = _empty_letter;
            for (int i = 0; i < width * height; i++)
            {
                screenString[i] = empty_letter;
                letterColorScreen[i] = letterColor;
                backgroundColorScreen[i] = backgroundColor;
            }
            hasNoColorChange = true;
            letterAndBackColor = [letterColor, backgroundColor];
            bufferArray = new char[width];
        }

    }
    public class Debugger
    {
        private string currentTab;
        private string tab = "    ";

        private List<string> history = new List<string>();
        public void AlertError()
        {
            Console.WriteLine("!!!!!Error!!!!!");
            Console.WriteLine("Something bad detected by this program...");
            Console.WriteLine("Press Enter to Exit.");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {

            }
            Environment.Exit(0);
        }
        public void AlertError(string message)
        {

            Console.WriteLine("!!!!!Error!!!!!");
            Console.WriteLine("Something bad detected by this program...");
            Console.WriteLine("This might be help -> " + message);
            Console.WriteLine("Press Enter to Exit.");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {

            }
            Environment.Exit(0);
        }
        public void AlertError(string valueName, string value)
        {

            Console.WriteLine("!!!!!Error!!!!!");
            Console.WriteLine("Something bad detected by this program...");
            Console.WriteLine("This might be help -> " + valueName + " : " + value);
            Console.WriteLine("Press Enter to Exit.");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {

            }
            Environment.Exit(0);
        }
        public void DebugVector(float[] vector)
        {
            Console.Write("[");
            for (int i = 0; i < vector.Length - 1; i++)
            {
                Console.Write(vector[i] + ", ");
            }
            Console.Write(vector[vector.Length - 1]);
            Console.WriteLine("]");
        }
        public string DebugVector_string(float[] vector)
        {
            string data = "[";
            for (int i = 0; i < vector.Length - 1; i++)
            {
                data += vector[i].ToString() + ", ";
            }
            data += vector[vector.Length - 1].ToString() + "]";
            return data;
        }
        public void DebugMonoTree(Mono root)
        {
            Output(Environment.NewLine);
            if (root.HasComponent<Transform>() == true)
            {
                Output("name : " + root.GetName());
                DebugTransform(root.GetComponent<Transform>());
            }
            if (root.HasChild() == true)
            {
                List<Mono> children = new List<Mono>();
                children = root.GetChildren();
                currentTab += tab;
                for (int i = 0; i < root.GetChildCount(); i++)
                {
                    DebugMonoTree(children[i]);
                }
                currentTab.Remove(0, 4);
            }
        }
        public void DebugTransform(Transform trans)
        {
            Output("position : " + DebugVector_string(trans.GetPosition()));
            Output("rotation : " + DebugVector_string(trans.GetRotation()));
            Output("worldPos : " + DebugVector_string(trans.GetWorldPosition()));
        }
        public void DebugTransformTree(Transform root)
        {
            Output(Environment.NewLine);
            DebugTransform(root);
            if (root.HasChild())
            {
                List<Transform> children = new List<Transform>();
                children = root.GetChildren();
                currentTab += tab;
                for (int i = 0; i < children.Count; i++)
                {
                    DebugTransformTree(children[i]);
                }
                currentTab.Remove(0, 4);
            }
        }
        public void Output(string message)
        {
            //Console.WriteLine(currentTab + message);
            history.Add(currentTab + message + Environment.NewLine);
        }
        public void ShowHistory(int startIndex, int endIndex)
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                if (i < history.Count && i >= 0)
                {
                    Console.Write(history[i]);
                }
            }
        }
    }
}

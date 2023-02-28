
using ZedGraph;

namespace _8B6T_NRZ_LogicalEncoder_V2 {
    public partial class Form1 : Form {
        public static List<string> HexCode = new List<string>();
        public static List<string> SignalCode = new List<string>();
        public static List<string> Signalinputs = new List<string>();
        public static ZedGraphControl zedGraph = new ZedGraphControl();
        public static PointPairList list = new PointPairList();
        public static GraphPane myPane = zedGraph.GraphPane;
        public static LineItem myCurve = myPane.AddCurve("Signal", list, Color.Green, SymbolType.Diamond);

        public Form1() {
            InitializeComponent();

            this.Text = "Encoder 8B/6T NRZ";
            zedGraph.Location = new System.Drawing.Point(100, 100);
            zedGraph.Name = "zedGraph";
            zedGraph.Size = new System.Drawing.Size(600, 250);
            zedGraph.IsEnableZoom = false;
            InitializeCodeTableString();

            this.Controls.Add(zedGraph);
            CreateGraph(zedGraph);
        }

        private static void InitializeCodeTableString() {
            string[] stringComponents;
            try {
                using (StreamReader Reader = new StreamReader(".//CodeTable.txt")) {
                    while (!Reader.EndOfStream) {
                        stringComponents = Reader.ReadLine().Split(' ');
                        if (stringComponents[0] != "") {
                            for (int i = 0; i < stringComponents.Length; i += 2) {
                                HexCode.Add(stringComponents[i]);
                                SignalCode.Add(stringComponents[i + 1]);
                            }
                        }
                    }
                }
            }
            catch {
                MessageBox.Show("CodeTable does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);

            }
        }

        private static string Bin2Hex(string str) {
            long temp = Convert.ToInt64(str, 2);
            string result = Convert.ToString(temp, 16);
            return result.ToUpper();
        }

        private static void CreateGraph(ZedGraphControl zgc) {
            GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "";
            myPane.XAxis.Title.Text = "";
            myPane.YAxis.Title.Text = "";
            myPane.YAxis.Scale.Max = 1.5;
            myPane.YAxis.Scale.Min = -1.5;
            myPane.XAxis.Scale.Min = 0;
            myPane.YAxis.Scale.MajorStep = 1;
            myPane.YAxis.Scale.MinorStep = 1;
            myPane.XAxis.Scale.MajorStep = 1;
            myPane.XAxis.Scale.MinorStep = 1;
            
        }

        private void button1_Click(object sender, EventArgs e) {
            string userBinCode = textBox1.Text;
            int chr = 0;
            for (int i = 0; i < userBinCode.Length; i++)
                if (userBinCode[i] == '0' || userBinCode[i] == '1') { }
                else {
                    MessageBox.Show("Wrong input", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            while ((userBinCode.Length % 8) != 0) {
                userBinCode += "0";
            }
            string temp = userBinCode;

            for (int i = 8; i < userBinCode.Length; i += 9) {
                temp = temp.Insert(i, " ");
            }
            label1.Text = temp;
            textBox1.Text = userBinCode;
            string[] inputs = temp.Split(' ');
            Signalinputs.Clear();
            if (textBox1.Text == "") {
                myCurve.Clear();
                zedGraph.AxisChange();
                zedGraph.Refresh();
                return;
            }
            for (int i = 0; i < inputs.Length; i++) {
                Signalinputs.Add(SignalCode[HexCode.IndexOf(Bin2Hex(inputs[i]))]);
            }
            myCurve.Clear();

            for (int i = 0; i < Signalinputs.Count; i++) {

                for (int j = 0; j < 6; j++) {
                    switch (Signalinputs[i][j]) {
                        case '+':
                            list.Add(j + (i * 6), 1);
                            list.Add(j + (i * 6) + 1, 1);
                            break;
                        case '0':
                            list.Add(j + (i * 6), 0);
                            list.Add(j + (i * 6) + 1, 0);
                            break;
                        case '–':
                            list.Add(j + (i * 6), -1);
                            list.Add(j + (i * 6) + 1, -1);
                            break;
                    }
                }
            }

            myPane.Chart.Border.IsVisible = false;
            myPane.XAxis.MajorTic.IsOpposite = false;
            myPane.XAxis.MinorTic.IsOpposite = false;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;

            zedGraph.AxisChange();
            zedGraph.Refresh();
        }
    }
}
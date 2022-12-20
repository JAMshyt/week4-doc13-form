using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ПО_Ф {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		abstract class PO {
			public string Name;
			public string Company;
			public DateTime InstDate;
			public int UsePeriod;
			public int cost;

			public PO(string name, string comp, string date, int period, int cos) {
				Name = name;
				Company = comp;
				InstDate = Convert.ToDateTime(date);
				UsePeriod = period;
				cost = cos;
			}
			public virtual void Show(ref string t) {
				t = ("Имя: " + Name + "\n" +
					"Производитель: " + Company + "\n" +
					"Дата установки: " + InstDate.ToShortDateString() + "\n" +
					"Пробный период (дней): " + UsePeriod + "\n");
			}
			public virtual bool canUse() {
				if (DateTime.Now < this.InstDate.AddDays(this.UsePeriod)) return true;
				else return false;
			}
		}

		class Free : PO {
			public Free(string name, string comp) : base(name, comp, "1/1/1", 0, 0) { }

			public override void Show(ref string t) {
				t = ("Имя: " + Name + "\n" +
					"Производитель: " + Company);
			}
			public override bool canUse() {
				return true;
			}
		}

		class Shareware : PO {
			public Shareware(string name, string comp, string date, int per) : base(name, comp, date, per, 0) { }

			public override void Show(ref string t) {
				base.Show(ref t);
			}
			public override bool canUse() {
				return base.canUse();
			}
		}

		class Com : PO {
			int period;
			public Com(string name, string comp, string date, int cost, int period) : base(name, comp, date, 0, cost) {
				this.period = period;
			}

			public override void Show(ref string t) {
				t = ("Имя: " + Name + "\n" +
					"Производитель: " + Company + "\n" +
					"Дата установки: " + InstDate.ToShortDateString() + "\n" +
					"Период: " + period + "\n" +
					"Цена: " + cost);
			}
			public override bool canUse() {
				if (DateTime.Now < InstDate.AddDays(period)) return true;
				else return false;
			}
		}


		private void button2_Click(object sender, EventArgs e) {
			richTextBox2.Text = "";
			string t = "";
			StreamReader reader = new StreamReader("..\\..\\..\\info.txt");
			string[] f = reader.ReadToEnd().Split('\n');
			string[][] arr = new string[f.Length][];
			PO[] po2 = new PO[f.Length];
			try {
				for (int i = 0; i < f.Length; i++) {
					arr[i] = f[i].Split(',');
				}
				for (int i = 0; i < arr.Length; i++) {
					for (int j = 0; j < arr[i].Length; j++) {
						if (arr[i].Length == 2) {
							po2[i] = new Free(arr[i][0], arr[i][1]);
							i++;
						}
						if (arr[i].Length == 4) {
							po2[i] = new Shareware(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][3]));
						}
						if (arr[i].Length == 5) {
							po2[i] = new Com(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][4]), Convert.ToInt32(arr[i][3]));
						}
					}
				}
			}
			catch { MessageBox.Show("Ошибка"); }
			try {
				richTextBox2.Text += "\n Список всех ПО:\n";
				foreach (PO ee in po2) {
					richTextBox2.Text += "\n------------------------\n";
					ee.Show(ref t);
					richTextBox2.Text += t + "\n";
					richTextBox2.Text += "------------------------\n";
				}
			}
			catch { MessageBox.Show("Пусто"); }

		}

		private void button1_Click(object sender, EventArgs e) {
			richTextBox2.Text = "";
			string t = "";
			string[] f = richTextBox1.Text.Split('\n');
			string[][] arr = new string[f.Length][];
			for (int i = 0; i < f.Length; i++) {
				arr[i] = f[i].Split(',');
			}
			PO[] p = new PO[f.Length];
			for (int i = 0; i < arr.Length; i++) {
				for (int j = 0; j < arr[i].Length; j++) {
					if (arr[i].Length == 2) {
						p[i] = new Free(arr[i][0], arr[i][1]);
						i++;
					}
					if (arr[i].Length == 4) {
						p[i] = new Shareware(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][3]));
					}
					if (arr[i].Length == 5) {
						p[i] = new Com(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][4]), Convert.ToInt32(arr[i][3]));
					}
				}
			}
			try {
				richTextBox2.Text += "\n Список всех ПО:\n";
				foreach (PO ee in p) {
					richTextBox2.Text += "\n------------------------\n";
					ee.Show(ref t);
					richTextBox2.Text += t + "\n";
					richTextBox2.Text += "------------------------\n";
				}
				richTextBox2.Text += "\nСписок доступных ПО:";
				foreach (PO ee in p) {
					if (ee.canUse()) {
						richTextBox2.Text += "\n------------------------\n";
						ee.Show(ref t);
						richTextBox2.Text += t + "\n";
						richTextBox2.Text += "------------------------\n";
					}
				}
				richTextBox2.Text += "\n";
			}
			catch {MessageBox.Show("Ошибка");}
		}

		private void button3_Click(object sender, EventArgs e) {
			richTextBox2.Text = "";
			string t = "";
			StreamReader reader = new StreamReader("..\\..\\..\\info.txt");
			string[] f = reader.ReadToEnd().Split('\n');
			string[][] arr = new string[f.Length][];
			PO[] po2 = new PO[f.Length];
			for (int i = 0; i < f.Length; i++) {
				arr[i] = f[i].Split(',');
			}
			for (int i = 0; i < arr.Length; i++) {
				for (int j = 0; j < arr[i].Length; j++) {
					if (arr[i].Length == 2) {
						po2[i] = new Free(arr[i][0], arr[i][1]);
						i++;
					}
					if (arr[i].Length == 4) {
						po2[i] = new Shareware(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][3]));
					}
					if (arr[i].Length == 5) {
						po2[i] = new Com(arr[i][0], arr[i][1], arr[i][2], Convert.ToInt32(arr[i][4]), Convert.ToInt32(arr[i][3]));
					}
				}
			}
			try {
				richTextBox2.Text += "\nСписок доступных ПО на сегодня:\n";
				foreach (PO ee in po2) {
					if (ee.canUse()) {
						richTextBox2.Text += "\n------------------------\n";
						ee.Show(ref t);
						richTextBox2.Text += t + "\n";
						richTextBox2.Text += "------------------------\n";
					}
				}
			}
			catch { MessageBox.Show("Пусто"); }
		}
	}
}



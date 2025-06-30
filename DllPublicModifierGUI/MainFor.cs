using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace DllPublicModifierGUI
{
    public partial class MainForm : Form
    {
        // Элементы управления
        private TextBox txtDllPath;
        private Button btnSelectDll;
        private ListBox lstFields;
        private ListBox lstMethods;
        private ListBox lstClasses;
        private Button btnModify;
        private Label lblStatus;
        private TabControl tabControl;
        private TabPage tabFields;
        private TabPage tabMethods;
        private TabPage tabClasses;

        public MainForm()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "DLL Access Modifier";
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.MinimumSize = new System.Drawing.Size(600, 400);

            txtDllPath = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(12, 12),
                Size = new System.Drawing.Size(550, 20),
                ReadOnly = true
            };

            btnSelectDll = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new System.Drawing.Point(568, 10),
                Size = new System.Drawing.Size(120, 23),
                Text = "Выбрать DLL"
            };
            btnSelectDll.Click += btnSelectDll_Click;

            tabControl = new TabControl
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(12, 40),
                Size = new System.Drawing.Size(676, 400)
            };

            tabFields = new TabPage("Поля");
            tabMethods = new TabPage("Методы");
            tabClasses = new TabPage("Классы/Типы");

            lstFields = new ListBox { Dock = DockStyle.Fill, SelectionMode = SelectionMode.MultiExtended };
            lstMethods = new ListBox { Dock = DockStyle.Fill, SelectionMode = SelectionMode.MultiExtended };
            lstClasses = new ListBox { Dock = DockStyle.Fill, SelectionMode = SelectionMode.MultiExtended };

            tabFields.Controls.Add(lstFields);
            tabMethods.Controls.Add(lstMethods);
            tabClasses.Controls.Add(lstClasses);

            tabControl.TabPages.Add(tabFields);
            tabControl.TabPages.Add(tabMethods);
            tabControl.TabPages.Add(tabClasses);

            btnModify = new Button
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new System.Drawing.Point(568, 450),
                Size = new System.Drawing.Size(120, 23),
                Text = "Сделать public",
                Enabled = false
            };
            btnModify.Click += btnModify_Click;

            lblStatus = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                Location = new System.Drawing.Point(12, 455),
                Text = "Выберите DLL..."
            };

            this.Controls.Add(txtDllPath);
            this.Controls.Add(btnSelectDll);
            this.Controls.Add(tabControl);
            this.Controls.Add(btnModify);
            this.Controls.Add(lblStatus);
        }

        private void btnSelectDll_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDllPath.Text = openFileDialog.FileName;
                    LoadDllInfo(openFileDialog.FileName);
                }
            }
        }

        private void LoadDllInfo(string dllPath)
        {
            lstFields.Items.Clear();
            lstMethods.Items.Clear();
            lstClasses.Items.Clear();

            try
            {
                var assemblyDefinition = AssemblyDefinition.ReadAssembly(dllPath);
                int restrictedFields = 0;
                int restrictedMethods = 0;
                int restrictedClasses = 0;

                foreach (var type in assemblyDefinition.MainModule.GetAllTypes())
                {
                    if (type.IsNestedPrivate || type.IsNestedAssembly || !type.IsPublic)
                    {
                        string typeAccess = type.IsNestedPrivate ? "private" :
                                          type.IsNestedAssembly ? "internal" :
                                          "non-public";
                        lstClasses.Items.Add($"{type.FullName} ({typeAccess})");
                        restrictedClasses++;
                    }

                    foreach (var field in type.Fields.Where(f => f.IsPrivate || f.IsAssembly))
                    {
                        lstFields.Items.Add($"{type.FullName}.{field.Name} ({GetAccessModifier(field)})");
                        restrictedFields++;
                    }

                    foreach (var method in type.Methods.Where(m => m.IsPrivate || m.IsAssembly))
                    {
                        lstMethods.Items.Add($"{type.FullName}.{method.Name} ({GetAccessModifier(method)})");
                        restrictedMethods++;
                    }
                }

                lblStatus.Text = $"Найдено: {restrictedClasses} классов/типов, {restrictedFields} полей и {restrictedMethods} методов с ограниченным доступом";
                btnModify.Enabled = (restrictedClasses + restrictedFields + restrictedMethods) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки DLL: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetAccessModifier(FieldDefinition field)
        {
            if (field.IsPrivate) return "private";
            if (field.IsAssembly) return "internal";
            if (field.IsPublic) return "public";
            return "protected";
        }

        private string GetAccessModifier(MethodDefinition method)
        {
            if (method.IsPrivate) return "private";
            if (method.IsAssembly) return "internal";
            if (method.IsPublic) return "public";
            return "protected";
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDllPath.Text) || !File.Exists(txtDllPath.Text))
            {
                MessageBox.Show("Файл DLL не выбран или не существует", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string outputFolder = Path.Combine(desktopPath, "Public_dll");

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                var assemblyDefinition = AssemblyDefinition.ReadAssembly(txtDllPath.Text);
                int modifiedFields = 0;
                int modifiedMethods = 0;
                int modifiedClasses = 0;

                foreach (var type in assemblyDefinition.MainModule.GetAllTypes())
                {
                    if (type.IsNestedPrivate || type.IsNestedAssembly || !type.IsPublic)
                    {
                        if (type.IsNested)
                        {
                            type.IsNestedPrivate = false;
                            type.IsNestedAssembly = false;
                            type.IsNestedPublic = true;
                        }
                        else
                        {
                            type.IsPublic = true;
                        }
                        modifiedClasses++;
                    }

  
                    foreach (var field in type.Fields.Where(f => f.IsPrivate || f.IsAssembly))
                    {
                        field.IsPrivate = false;
                        field.IsAssembly = false;
                        field.IsPublic = true;
                        modifiedFields++;
                    }

                    foreach (var method in type.Methods.Where(m => m.IsPrivate || m.IsAssembly))
                    {
                        method.IsPrivate = false;
                        method.IsAssembly = false;
                        method.IsPublic = true;
                        modifiedMethods++;
                    }
                }

                string outputFileName = $"modified_{Path.GetFileName(txtDllPath.Text)}";
                string outputPath = Path.Combine(outputFolder, outputFileName);
                assemblyDefinition.Write(outputPath);

                MessageBox.Show($"Успешно модифицировано:\n" +
                               $"- {modifiedClasses} классов/типов\n" +
                               $"- {modifiedFields} полей\n" +
                               $"- {modifiedMethods} методов\n\n" +
                               $"Новая DLL сохранена в:\n{outputPath}",
                               "Готово",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);

                LoadDllInfo(outputPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при модификации DLL: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
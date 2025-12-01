// Maded by Pedro M Marangon
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ScrollingCreditsScreen.Editor
{
	public class CreditsEditorWindow : EditorWindow
	{
		#region Constants
		private const string WINDOW_TITLE = "Credits Editor";
		private const string LINE_BREAK_INDICATOR = "[Line Break]";
		
		private const string LABEL_GAME_NAME = "Game Name";
		
		private const string BUTTON_LOAD_FILE = "Load From File";
		private const string BUTTON_SAVE_TO_EXISTING = "Save Credits to Existing File";
		private const string BUTTON_SAVE_TO_NEW_FILE = "Save Credits to New File";
		private const string BUTTON_ROLE = "Role";
		private const string BUTTON_PERSON = "Person";
		private const string BUTTON_LINE_BREAK = "Line Break";
		private const string BUTTON_ADD = "+";
		private const string BUTTON_REMOVE = "-";
		private const string BUTTON_RESET = "Reset";

		private const string SAVE_DIALOG_TITLE = "Save Credits File";
		private const string SAVE_DIALOG_BASE_FOLDER = "Assets";
		private const string SAVE_DIALOG_DEFAULT_NAME = "My Game Credits";
		private const string SAVE_DIALOG_EXTENSION = ".txt";

		private const string NULL_STRING = null;

		private const float SPACING = 10;
		#endregion

		private static string gameName = LABEL_GAME_NAME;
		private List<Role> roles = new List<Role>();
		private Vector2 scrollPosition;
		private TextAsset creditsFile = null;

		
		[MenuItem("Tools/Credits Editor")]
		public static void ShowWindow()
		{
			GetWindow<CreditsEditorWindow>(WINDOW_TITLE);
		}

		private void OnGUI()
		{
			GUILayout.Label(WINDOW_TITLE, EditorStyles.boldLabel);

			GUILayout.Space(SPACING);

			DrawFileButtons();

			GUILayout.Space(SPACING);
			
			gameName = EditorGUILayout.TextField(LABEL_GAME_NAME, gameName);

			GUILayout.Space(SPACING);

			using (new EditorGUILayout.ScrollViewScope(scrollPosition, GUI.skin.box))
			{
				for (int i = 0; i < roles.Count; i++)
				{
					DrawRoleSection(roles[i], i);
				}

				if (GUILayout.Button($"{BUTTON_ADD}{BUTTON_ROLE}")) roles.Add(new Role());

			}

			GUILayout.Space(SPACING);
		}

		
		//------ Drawing Functions -------//

		private void DrawRoleSection(Role role, int index)
		{
			using (new EditorGUILayout.VerticalScope(GUI.skin.box))
			{
				role.RoleName = EditorGUILayout.TextField(BUTTON_ROLE, role.RoleName);

				GUILayout.Space(SPACING);

				EditorGUI.indentLevel++;
				for (int i = 0; i < role.Persons.Count; i++)
				{
					string person = role.Persons[i];
					if (person == LINE_BREAK_INDICATOR)
					{
						GUILayout.Label(LINE_BREAK_INDICATOR, EditorStyles.miniLabel);
						continue;
					}					
					role.Persons[i] = EditorGUILayout.TextField(NULL_STRING, person);
				}
				EditorGUI.indentLevel--;
				
				DrawRoleButtons(role, index);
			}
		}

		private void DrawRoleButtons(Role role, int index)
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button($"{BUTTON_ADD} {BUTTON_PERSON}")) role.Persons.Add(string.Empty);

				if (GUILayout.Button($"{BUTTON_ADD} {BUTTON_LINE_BREAK}")) role.Persons.Add(LINE_BREAK_INDICATOR);


				bool hasPersons = role.Persons != null && role.Persons.Count > 0;
				using (new EditorGUI.DisabledGroupScope(!hasPersons))
				{
					string lastItem = hasPersons ?
						(role.Persons[^1] == LINE_BREAK_INDICATOR ? BUTTON_LINE_BREAK : BUTTON_PERSON) :
						BUTTON_PERSON;
					if (GUILayout.Button($"{BUTTON_REMOVE} {lastItem}"))
					{
						role.Persons.RemoveAt(role.Persons.Count - 1);
					}
				}

				if (GUILayout.Button($"{BUTTON_REMOVE} {BUTTON_ROLE}"))
				{
					roles.RemoveAt(index);
				}
			}


			GUILayout.Space(SPACING);
		}

		private void DrawFileButtons()
		{
			using (new GUILayout.VerticalScope(GUI.skin.box))
			{
				using (new GUILayout.HorizontalScope())
				{
					creditsFile = EditorGUILayout.ObjectField(GUIContent.none, creditsFile, typeof(TextAsset), false) as TextAsset;

					if (GUILayout.Button(BUTTON_LOAD_FILE))
					{
						LoadCreditsFile(creditsFile);
					}
				}


				string saveButtonLabel = creditsFile == null ? BUTTON_SAVE_TO_NEW_FILE : BUTTON_SAVE_TO_EXISTING;
				if (GUILayout.Button(saveButtonLabel))
				{
					SaveCreditsFile(creditsFile);
				}

				if (GUILayout.Button(BUTTON_RESET))
				{
					ResetCredits();
				}
			}
		}

		
		//------ GUI Buttons Logic -------//

		private void LoadCreditsFile(TextAsset creditsFile)
		{
			string[] lines = creditsFile.text.Split(CreditsScreen.LINE_BREAK);

			gameName = PlayerSettings.productName;
			roles.Clear();

			Role currentSection = null;

			foreach (var line in lines)
			{
				string trimmedLine = line.Trim();

				currentSection = ProcessLinesFromFile(trimmedLine, currentSection);
			}

			if (currentSection != null)
				roles.Add(currentSection);
		}

		private void SaveCreditsFile(TextAsset creditsFile)
		{
			string path = creditsFile == null ?
				EditorUtility.SaveFilePanel(SAVE_DIALOG_TITLE, SAVE_DIALOG_BASE_FOLDER, SAVE_DIALOG_DEFAULT_NAME, SAVE_DIALOG_EXTENSION) :
				Path.Combine(Path.GetDirectoryName(Application.dataPath), AssetDatabase.GetAssetPath(creditsFile));

			if (string.IsNullOrEmpty(path))
				return;

			List<string> lines = new List<string>() { $"{CreditsScreen.GAME_TITLE_IDENTIFIER}{gameName}" };
			foreach (var roleSection in roles)
			{
				lines.Add($"{CreditsScreen.ROLE_IDENTIFIER}{roleSection.RoleName}");
				foreach (var person in roleSection.Persons)
				{
					if (person == LINE_BREAK_INDICATOR)
					{
						lines.Add($"{CreditsScreen.LINE_BREAK_IDENTIFIER}");
					}
					else
					{
						lines.Add($"{CreditsScreen.PERSON_IDENTIFIER}{person}");
					}
				}
			}

			File.WriteAllLines(path, lines);
			AssetDatabase.Refresh();
		}
		
		private void ResetCredits()
		{
			gameName = PlayerSettings.productName;
			roles.Clear();

			creditsFile = null;
		}

		
		//------ Helper Functions --------//

		private Role ProcessLinesFromFile(string trimmedLine, Role currentSection)
		{
			Debug.Log(trimmedLine);
			if (string.IsNullOrEmpty(trimmedLine))
			{
				return currentSection;
			}

			if (trimmedLine.StartsWith(CreditsScreen.GAME_TITLE_IDENTIFIER))
			{
				gameName = trimmedLine.Substring(1).Trim();
				return currentSection;
			}

			if (trimmedLine.StartsWith(CreditsScreen.ROLE_IDENTIFIER))
			{
				if (currentSection != null)
					roles.Add(currentSection);

				currentSection = new Role();
				currentSection.RoleName = trimmedLine.Substring(1).Trim();
				return currentSection;
			}

			if (trimmedLine.StartsWith(CreditsScreen.PERSON_IDENTIFIER))
			{
				if (currentSection == null)
					currentSection = new Role();

				currentSection.Persons.Add(trimmedLine.Substring(1).Trim());
				return currentSection;
			}

			if (trimmedLine.StartsWith(CreditsScreen.LINE_BREAK_IDENTIFIER))
			{
				if (currentSection == null)
					currentSection = new Role();

				currentSection.Persons.Add(LINE_BREAK_INDICATOR);

				return currentSection;
			}

			return currentSection;

		}


		//------- Custom Classes  --------//

		[System.Serializable]
		public class Role
		{
			public string RoleName = "";
			public List<string> Persons = new List<string>();
		}
	}
}
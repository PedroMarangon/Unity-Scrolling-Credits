// Maded by Pedro M Marangon
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScrollingCreditsScreen
{
	public class CreditsScreen : MonoBehaviour
	{
		#region Constants
		private const float CREDITS_AT_BOTTOM = 1;
		private const float CREDITS_AT_TOP = 0;

		private const int ACTUAL_LINE_START = 1;
		private const int IDENTIFIER_INDEX = 0;

		public const string LINE_BREAK = "\n";

		public const char GAME_TITLE_IDENTIFIER = '#';
		public const char ROLE_IDENTIFIER = '!';
		public const char PERSON_IDENTIFIER = '_';
		public const char LINE_BREAK_IDENTIFIER = '-';

		#region Tooltips
		private const string TOOLTIP_CREDITS_TEXT = "Use your own Credis Text Asset here. If you don't have one, use the \'Tools/Credits Editor\' window to create yours.";
		#endregion
		#endregion


		[Header("Settings")]
		[Range(0, 5), SerializeField] private float startWaitTime = 1f;
		[SerializeField] private float scrollDuration = 10;
		[Space]
		[Tooltip(TOOLTIP_CREDITS_TEXT), SerializeField] private TextAsset creditsTextAsset;
		[SerializeField] private UnityEvent OnCreditsEnded;

		[Space, Header("Customization")]
		[SerializeField] private Sprite backgroundSprite;
		[SerializeField] private Color darkeningColor = new Color(0.0622f, 0.0622f, 0.08f, 0.6f);
		[Space]
		[SerializeField] private Color gameTitleColor = new Color(0.855f, 0.314f, 0.212f);
		[SerializeField] private Color roleColor = new Color(0.894f, 0.647f, 0.384f);
		[SerializeField] private Color personColor = new Color(0.663f, 0.631f, 0.557f);

		[Space, Header("Texts")]
		[SerializeField] private TMP_Text gameTitle;
		[SerializeField] private TMP_Text role;
		[SerializeField] private TMP_Text people;

		[Space, Header("Images")]
		[SerializeField] private Image backgroundImage;
		[SerializeField] private Image darkeningImage;
				
		[Space, Header("Movement")]
		[SerializeField] private RectTransform creditsRectTransform;


		private void OnValidate()
		{
			if (Application.isPlaying) return;
			SetupGraphics();
		}

		private void Awake()
		{
			SetupTextMeshProObjects();
			SetupGraphics();
		}

		private void Start()
		{
			if(creditsTextAsset == null)
			{
				Debug.LogError("[Start] Error: Credits text asset field is empty. Assign a TextAsset that follows the correct format (use the Tools/Credits Editor window to create one.)", this);
				return;
			}

			if(creditsRectTransform == null)
			{
				Debug.LogError("Error: Credits RectTransform is empty. Assign a RectTransform outside of Edit Mode and play the scene again.", this);
				return;
			}

			Vector2 startPivot = new Vector2(creditsRectTransform.pivot.x, CREDITS_AT_BOTTOM);

			creditsRectTransform.pivot = startPivot;

			StartCoroutine(nameof(ScrollCreditsCoroutine), startPivot);
		}


		private IEnumerator ScrollCreditsCoroutine(Vector2 startPivot)
		{
			Vector2 targetPivot = new Vector2(startPivot.x, CREDITS_AT_TOP);
			
			yield return new WaitForSeconds(startWaitTime);

			float elapsedTime = 0f;

			while (elapsedTime < scrollDuration)
			{
				elapsedTime += Time.deltaTime;
				float t = Mathf.Clamp01(elapsedTime / scrollDuration);
				creditsRectTransform.pivot = Vector2.Lerp(startPivot, targetPivot, t);
				yield return null;
			}

			creditsRectTransform.pivot = targetPivot;

			OnCreditsEnded?.Invoke();
		}



		private void SetupGraphics()
		{
			if (backgroundImage != null) backgroundImage.sprite = backgroundSprite;
			if (darkeningImage != null) darkeningImage.color = darkeningColor;

			if (gameTitle != null) gameTitle.color = gameTitleColor;
			if (role != null) role.color = roleColor;
			if (people != null) people.color = personColor;
		}


		private void SetupTextMeshProObjects()
		{

			if(creditsTextAsset == null)
			{
				Debug.LogError("[AWAKE] Error: Credits text asset field is empty. Assign a TextAsset that follows the correct format (use the Tools/Credits Editor window to create one.)", this);
				return;
			}

			string[] lines = creditsTextAsset.text.Split(LINE_BREAK);

			(string gameTitleText, string roleText, string peopleText) = GenerateCreditsFromFile(lines);

			FixTextStartingWithLineBreak(ref peopleText);
			FixTextStartingWithLineBreak(ref gameTitleText);
			FixTextStartingWithLineBreak(ref roleText);

			if(gameTitle == null || role == null || people == null)
			{
				Debug.LogError("Error: At least one of the TextMeshPro objects is empty. Check and assign the missing TextMeshPro objects outside of Edit Mode.", this);
				return;
			}

			gameTitle.text = gameTitleText;
			role.text = roleText;
			people.text = peopleText;
		}

		private (string, string, string) GenerateCreditsFromFile(string[] lines)
		{
			string gameTitleText = string.Empty;
			string roleText = string.Empty;
			string peopleText = string.Empty;

			foreach (string line in lines)
			{
				if (line.Length <= IDENTIFIER_INDEX) continue;

				switch (line[IDENTIFIER_INDEX])
				{
					case GAME_TITLE_IDENTIFIER:
						gameTitleText = line.Substring(ACTUAL_LINE_START);
						break;
					case ROLE_IDENTIFIER:
						roleText += $"{line.Substring(ACTUAL_LINE_START)}{LINE_BREAK}";
						peopleText += LINE_BREAK;
						break;
					case PERSON_IDENTIFIER:
						roleText += LINE_BREAK;
						peopleText += $"{line.Substring(ACTUAL_LINE_START)}{LINE_BREAK}";
						break;
					case LINE_BREAK_IDENTIFIER:
						peopleText += LINE_BREAK;
						roleText += LINE_BREAK;
						break;
					default:
						break;
				}
			}

			return (gameTitleText, roleText, peopleText);
		}

		private void FixTextStartingWithLineBreak(ref string text)
		{
			if (!text.StartsWith(LINE_BREAK)) return;
			text = text.Substring(ACTUAL_LINE_START);
		}

	}
}
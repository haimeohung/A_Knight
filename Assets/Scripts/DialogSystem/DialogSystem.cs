using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;
using Assets.Scripts.DialogSystem;
using System.Collections.Generic;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialogManager;
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtCharacter;
    [SerializeField] private TextMeshProUGUI txtComment;
    [SerializeField] private bool useCoroutine;
    [SerializeField] private GameObject avatarObject;

    public event System.Action OnDialogEnd;

    private Sprite currentSprite;
    private string currentCharacter;
    private int currentDialogID = -1;

    private Image avatar;
    private VIDE_Assign dialogAssignObj;
    private Animator animator;
    private Dialog dialogData;
    private bool isDialogChanged;

    /// <summary>
    /// State hiện tại của dialog
    /// </summary>
    public DialogState State { get; private set; } = DialogState.NotActive;

    private void Awake()
    {
        this.avatar = this.avatarObject.GetComponent<Image>();
        this.animator = this.GetComponent<Animator>();
        //this.StartDialog("Home");
    }

    /// <summary>
    /// Gắn 1 hội thoại mới vào trong Dialog Object
    /// </summary>
    /// <param name="dialogName">Tên của đoạn đối thoại</param>
    public void AssignNewDialog(string dialogName)
    {
        if (!this.dialogAssignObj)
        {
            this.dialogAssignObj = this.dialogManager.GetComponent<VIDE_Assign>();
        }
        bool assigned = this.dialogAssignObj.AssignNew(dialogName);
        if (!assigned)
        {
            Debug.Log("Dialog not found");
            throw new System.Exception("Dialog name not found");
        }
    }

    /// <summary>
    /// Bắt đầu một đoạn hội thoại, đồng thời active DialogObject
    /// </summary>
    /// <param name="dialogName">Tên của đoạn hội thoại</param>
    public void StartDialog(string dialogName)
    {
        if (this.State != DialogState.NotActive)
        {
            throw new System.Exception("The dialog has already been started");
        }
        this.State = DialogState.Running;
        this.triggerActive(1);
        animator.SetBool("dialogActive", true);
        this.AssignNewDialog(dialogName);
        this.OnBeginDialog();
    }

    /// <summary>
    /// Đi tới câu tiếp theo của đoạn hội thoại
    /// </summary>
    public void GoNext()
    {
        if (!this.dialogData.IsEndOfComment || this.State != DialogState.Running)
            return;
        VD.Next();
        if (this.dialogData.IsEnd)
        {
            this.EndDialog();
        }
        if (this.isDialogChanged)
        {
            this.triggerCharacterAnimation(0);
        }
        else
        {
            this.triggerCommentCoroutine();
        }
    }

    /// <summary>
    /// Dừng một đoạn hội thoại, đồng thời deactivate DialogObject
    /// </summary>
    public void PauseDialog()
    {
        if (this.State != DialogState.Running)
        {
            return;
        }
        this.animator.SetBool("dialogActive", false);
        this.State = DialogState.Pause;
    }

    /// <summary>
    /// Tiếp tục đoạn hội thoại, đồng thời activate DialogObject. Hàm này chỉ hoạt động khi trước đó đã gọi PauseDialog
    /// </summary>
    public void ContinueDialog()
    {
        if (this.State != DialogState.Pause)
        {
            return;
        }
        this.State = DialogState.Running;
        this.triggerActive(1);
        this.animator.SetBool("dialogActive", true);
        this.GoNext();
    }

    /// <summary>
    /// Kết thúc trực tiếp 1 đoạn hội thoại, đồng thời deactivate DialogObject
    /// </summary>
    public void EndDialog()
    {
        this.dialogData.End();
        this.currentDialogID = -1;
        VD.EndDialogue();
        VD.OnNodeChange -= this.OnDialogChanged;
        OnDialogEnd?.Invoke();
        this.State = DialogState.NotActive;
        this.animator.SetBool("dialogActive", false);
    }

    public void resetDialogUI()
    {
        this.avatar.sprite = null;
        this.currentCharacter = null;
    }

    void triggerActive(int value)
    { 
        bool active = value == 1;
        gameObject.SetActive(active);
    }

    void triggerCharacterAnimation(int value)
    {
        bool active = value == 1;
        if (active && this.State != DialogState.Running)
        {
            return;
        }
        this.animator.SetBool("characterActive", active);
    }
    void triggerUpdateCharacter()
    {
        this.avatar.sprite = this.currentSprite;
        this.txtCharacter.text = this.currentCharacter;
    }

    void triggerCommentCoroutine()
    {
        if (this.useCoroutine)
        {
            StartCoroutine(this.coroutineComment());
        }
    }

    protected IEnumerator coroutineComment()
    {
        while (!this.dialogData.IsEndOfComment)
        {
            this.txtComment.text = dialogData.NextCharacter();
            yield return new WaitForSeconds(0.01f);
        }
        StopAllCoroutines();
    }

    protected void OnBeginDialog()
    {
        VD.OnNodeChange += this.OnDialogChanged;

        try
        {
            VD.BeginDialogue(dialogAssignObj);
        }
        catch
        {
            Debug.Log("Error");
        }
    }
    protected void OnDialogChanged(VD.NodeData data)
    {
        var character = ((string)data.extraVars["character"]).ToUpper();
        if (data.nodeID != this.currentDialogID)
        {
            this.currentSprite = data.sprite;
            this.currentCharacter = character;
            this.isDialogChanged = true;
        }
        else
        {
            this.isDialogChanged = false;
        }
        this.currentDialogID = data.nodeID;
        if (this.dialogData == null || this.dialogData.IsEnd)
        {
            this.dialogData = new Dialog(data.comments);
        }
        if (!this.useCoroutine)
        {
            if (!this.dialogData.IsStarted)
            {
                this.dialogData.Start();
                this.txtComment.text = this.dialogData.CurrentComment;
            }
            else
            {
                this.txtComment.text = this.dialogData.NextComment();
            }
        }
        else
        {
            if (!this.dialogData.IsStarted)
            {
                this.dialogData.StartCoroutine();
            }
            else
            {
                this.dialogData.NextCharacter();
            }
        }
    }

    
    public enum DialogState
    {
        NotActive,
        Running,
        Pause
    }
}

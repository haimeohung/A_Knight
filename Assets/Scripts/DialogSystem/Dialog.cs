using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DialogSystem
{
    public class Dialog
    {
        private string[] comments;
        private int currentCommentIndex;
        private int currentCharacterIndex;
        private string currentCoroutineComment;

        public bool IsEnd
        {
            get => this.IsLastComment && this.IsEndOfComment;
        }
        public bool IsStarted { get; private set; } = false;
        public bool IsLastComment
        {
            get => this.currentCommentIndex == this.comments.Length - 1;
        }
        public bool IsFirstComment
        {
            get => this.currentCommentIndex == 0;
        }
        public bool IsEndOfComment
        {
            get => this.CurrentCoroutineComment.Length == this.CurrentComment.Length;
        }
        public string CurrentComment
        {
            get => this.comments[this.currentCommentIndex];
        }
        public string CurrentCoroutineComment
        {
            get => this.currentCoroutineComment;
        }


        public Dialog(string[] comments)
        {
            this.comments = comments;
            this._SetUpComment();
        }

        public string GetCurrentComment(bool includeCoroutine = false)
        {
            if (includeCoroutine)
            {
                return this.currentCoroutineComment;
            }
            return this.comments[this.currentCommentIndex];
        }
        
        public void Start()
        {
            this.IsStarted = true;
            this.NextComment();
        }

        public void StartCoroutine()
        {

            this.IsStarted = true;
            this.NextCharacter();
        }

        public string NextComment()
        {
            if (this.IsEnd)
                throw new Exception("The dialog has not started yet");

            if (this.IsLastComment)
            {
                return null;
            }

            this._UpdateComment();
            return this.CurrentComment;
        }

        public string NextCharacter()
        {
            if (this.IsLastComment)
            {
                if (this.IsEndOfComment)
                {
                    return null;
                }
                this._UpdateCharacter();
  
                return this.CurrentCoroutineComment;
            }

            if (this.currentCharacterIndex != -1)
            {
                if (this.IsEndOfComment)
                {
                    this._UpdateComment();
                    return this.CurrentComment;
                }
            }
            else
            {
                this._UpdateComment();
                return this.CurrentCoroutineComment;
            }

            this._UpdateCharacter();
            return this.CurrentCoroutineComment;
        }

        public void End()
        {
            this.currentCommentIndex = this.comments.Length - 1;
            this.currentCharacterIndex = this.CurrentComment.Last();
            this.currentCoroutineComment = this.CurrentComment;
        }

        protected void _SetUpComment()
        {
            this.currentCommentIndex = -1;
            this.currentCharacterIndex = -1;
            this.currentCoroutineComment = "";
        }
        protected void _UpdateComment()
        {
            this.currentCommentIndex++;
            this.currentCharacterIndex = 0;
            this.currentCoroutineComment = "" + this.CurrentComment[0];
        }
        protected void _UpdateCharacter()
        {
            this.currentCharacterIndex++;
            this.currentCoroutineComment = $"{this.currentCoroutineComment}{this.CurrentComment[this.currentCharacterIndex]}";
        }
    }
}

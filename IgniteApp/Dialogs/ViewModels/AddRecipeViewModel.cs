using IgniteApp.Bases;
using IgniteApp.Shell.Recipe.ViewModels;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Dialogs.ViewModels
{
    public class AddRecipeViewModel : ViewModelBase
    {
        public new string TitleName => "添加配方";
        private string _inputName;

        public string InputName
        {
            get { return _inputName; }
            set { SetAndNotify(ref _inputName, value); }
        }

        private string _type;

        public string InputTypeName
        {
            get { return _type; }
            set { SetAndNotify(ref _type, value); }
        }

        private int? _amount;

        public int? InputAmount
        {
            get { return _amount; }
            set { SetAndNotify(ref _amount, value); }
        }

        private decimal? _price;

        public decimal? InputPrice
        {
            get { return _price; }
            set { SetAndNotify(ref _price, value); }
        }

        private string _tag;

        public string InputTag
        {
            get { return _tag; }
            set { SetAndNotify(ref _tag, value); }
        }

        private DateTime _dateValue = DateTime.Now;

        public DateTime DateValue
        {
            get { return _dateValue; }
            set { SetAndNotify(ref _dateValue, value); }
        }

        private int? _userId;

        public int? InputUserId
        {
            get { return _userId; }
            set { SetAndNotify(ref _userId, value); }
        }

        private string _username;

        public string InputUserName
        {
            get { return _username; }
            set { SetAndNotify(ref _username, value); }
        }

        private readonly IRecipeRepository db;

        public AddRecipeViewModel(IRecipeRepository db)
        {
            this.db = db;
        }

        //输出关闭子窗体的返回结果
        public void ExecuteSave()
        {
            RecipeDto dto = new RecipeDto();
            dto.RecipeName = InputName;
            dto.IsDeleted = false;
            dto.CreateTime = DateTime.Now;
            dto.UpdateTime = DateTime.Now;
            dto.Remark = InputTag;
            dto.ViewName = InputTag;
            db.AddRecipe(dto);
            if (Parent is RecipeViewModel parent)
            {
                //parent.re
            }
            this.RequestClose(true);
        }

        public void ExecuteClose()
        {
            this.RequestClose(false);
        }
    }
}
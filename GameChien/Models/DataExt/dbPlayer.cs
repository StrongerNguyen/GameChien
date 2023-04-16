using GameChien.Models.Data;
using GameChien.Models.Dto;
using GameChien.Models.FormModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Ext
{
    public class dbPlayer
    {
        public static ResultModel<UserSessionModel> Login(string UserName, string Password)
        {
            using (var db = new GameChienEntities())
            {
                tblPlayer player = db.tblPlayers.FirstOrDefault(t => t.AccountName.Equals(UserName) && t.Password.Equals(Password));
                if (player != null)
                {
                    if (player.isBlock) return new ResultModel<UserSessionModel>(false, "Tài khoản đang bị tạm khóa.", null);
                    return new ResultModel<UserSessionModel>(true, "", new UserSessionModel()
                    {
                        UserName = player.AccountName,
                        UserId = player.Id
                    });
                }
                else return new ResultModel<UserSessionModel>(false, "Tài khoản hoặc mật khẩu không chính xác.", null);
            }
        }
        public static ResultModel Register(RegisterFormModel registerForm)
        {
            //Validate
            if (string.IsNullOrEmpty(registerForm.AccountName)) return new ResultModel(false, "Tên tài khoản không trống.");
            else if (string.IsNullOrEmpty(registerForm.Password)) return new ResultModel(false, "Mật khẩu không trống.");
            else if (string.IsNullOrEmpty(registerForm.FullName)) return new ResultModel(false, "Họ và tên không trống.");
            else if (string.IsNullOrEmpty(registerForm.PhoneNumber)) return new ResultModel(false, "Số điện thoại không trống.");
            else if (string.IsNullOrEmpty(registerForm.GameAccount)) return new ResultModel(false, "Tài khoản game không trống.");
            else if (!registerForm.Password.Equals(registerForm.RetypePassword)) return new ResultModel(false, "Nhập lại mật khẩu không trùng khớp.");
            else if (registerForm.PhoneNumber.Length != 10 || registerForm.PhoneNumber[0] != '0') return new ResultModel(false, "Số điện thoại không đúng.");
            else if (registerForm.AccountName.Length < 5) return new ResultModel(false, "Tên tài khoản dài tối thiểu 5 ký tự.");
            else if (registerForm.Password.Length < 6) return new ResultModel(false, "Mật khẩu dài tối thiểu 6 ký tự.");
            using (var db = new GameChienEntities())
            {
                if (db.tblPlayers.Count(t => t.AccountName.Equals(registerForm.AccountName)) > 0) return new ResultModel(false, "Tài khoản đã tồn tại, vui lòng đặt tên tài khoản khác.");
                if (db.tblPlayers.Count(t => t.GameAccount.Equals(registerForm.GameAccount)) > 0) return new ResultModel(false, "Tài khoản InGame đã tồn tại, vui lòng kiểm tra lại.");
                tblPlayer player = new tblPlayer()
                {
                    AccountName = registerForm.AccountName,
                    Password = registerForm.Password,
                    FullName = registerForm.FullName,
                    PhoneNumber = registerForm.PhoneNumber,
                    GameAccount = registerForm.GameAccount,
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTime.Now,
                    GameLevel = 0,
                    Credit = 0,
                    isBlock = false,
                    isVerifiedGameAccount = false,
                    PercentOfLevelUp = 0,
                    Avatar = "~/Content/Images/noavatar.png"
                };
                db.tblPlayers.Add(player);
                db.SaveChanges();
                return new ResultModel(true, "");
            }
        }
        public static PlayerDto GetById(Guid Id)
        {
            using (var db = new GameChienEntities())
            {
                tblPlayer player = db.tblPlayers.FirstOrDefault(t => t.Id.Equals(Id));
                if (player == null) return null;
                else return new PlayerDto()
                {
                    Id = player.Id,
                    AccountName = player.AccountName,
                    Avatar = player.Avatar,
                    Credit = player.Credit,
                    FullName = player.FullName,
                    GameAccount = player.GameAccount,
                    GameLevel = player.GameLevel,
                    PercentOfLevelUp = player.PercentOfLevelUp,
                    PhoneNumber = player.PhoneNumber,
                    Status = player.Status,
                    isBlock = player.isBlock,
                    isVerifiedGameAccount = player.isVerifiedGameAccount
                };
            }
        }
        public static tblPlayer GetByAccountName(string AccountName)
        {
            using (var db = new GameChienEntities())
            {
                return db.tblPlayers.FirstOrDefault(t => t.AccountName.Equals(AccountName));
            }
        }
        public static PlayerDto GetPlayerDtoByAccountName(string AccountName)
        {
            using (var db = new GameChienEntities())
            {
                tblPlayer player = db.tblPlayers.FirstOrDefault(t => t.AccountName.Equals(AccountName));
                if (player == null) return null;
                else return new PlayerDto()
                {
                    Id = player.Id,
                    AccountName = player.AccountName,
                    Avatar = player.Avatar,
                    Credit = player.Credit,
                    FullName = player.FullName,
                    GameAccount = player.GameAccount,
                    GameLevel = player.GameLevel,
                    PercentOfLevelUp = player.PercentOfLevelUp,
                    PhoneNumber = player.PhoneNumber,
                    Status = player.Status
                };
            }
        }
        public tblPlayer GetByGameAccount(string GameAccount)
        {
            using (var db = new GameChienEntities())
            {
                return db.tblPlayers.FirstOrDefault(t => t.GameAccount.Equals(GameAccount));
            }
        }
    }
}
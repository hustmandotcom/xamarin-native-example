using Foundation;
using PracticalCodingTest.Data;
using PracticalCodingTest.Handlers;
using PracticalCodingTest.Handlers.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace PracticalCodingTest
{
    public partial class UserTableViewController : UITableViewController
    {
        private IUserRepository _userRepository;
        private const string _cellIdentifier = "TableCell";
        private User[] _usersCache;

        #region Class

        public UserTableViewController(IntPtr handle) : base(handle)
        {
            _userRepository = new UserRepository(new InMemoryDatabase());
            _usersCache = _userRepository.Users.ToArray();
        }

        public override void ViewWillAppear(bool animated)
        {
            RefreshData();
            base.ViewWillAppear(animated);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier.Equals("AddUserViewSegue"))
                if (segue.DestinationViewController is AddUserViewController addUserViewController)
                    addUserViewController.UserRepository = _userRepository;

            base.PrepareForSegue(segue, sender);
        }

        #endregion

        #region Load Table

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _usersCache.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(_cellIdentifier) 
                ?? new UITableViewCell(UITableViewCellStyle.Default, _cellIdentifier);

            var user = _usersCache[indexPath.Row];
            cell.TextLabel.Text = user.Username;
            return cell;
        }

        #endregion

        #region Private

        private void RefreshData()
        {
            _usersCache = _userRepository.Users.ToArray();
            UserTableView.ReloadData();
        }

        #endregion

    }
}
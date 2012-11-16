using System.Collections.ObjectModel;
using System.Linq;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class IssueFilterViewModel
    {
        private readonly BoardService _boardService;

        public IssueFilterViewModel(BoardService boardService)
        {
            _boardService = boardService;
        }

        public IssueFilter ToModel()
        {
            return new IssueFilter();
        }
    }
}

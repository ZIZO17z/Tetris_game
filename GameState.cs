namespace DummiesV2
{
    public class GameState
    {
        public Block CurrentBlock { get; private set; }
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }

        private readonly int[] scoreValues = new int[] { 0, 100, 300, 500, 800 };

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            GameOver = false;
            Score = 0;
        }

        private bool IsBlockInValidPosition()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsInside(p.Row, p.Column))
                {
                    return false;
                }
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        private void LockBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            int clearedRows = GameGrid.ClearFullRows();
            Score += scoreValues[clearedRows];

            CurrentBlock = BlockQueue.GetAndUpdate();

            if (!IsBlockInValidPosition())
            {
                GameOver = true;
            }
        }

        public void MoveBlockLeft()
        {
            if (GameOver) return;
            CurrentBlock.Move(0, -1);
            if (!IsBlockInValidPosition())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            if (GameOver) return;
            CurrentBlock.Move(0, 1);
            if (!IsBlockInValidPosition())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        public void MoveBlockDown()
        {
            if (GameOver) return;
            CurrentBlock.Move(1, 0);
            if (!IsBlockInValidPosition())
            {
                CurrentBlock.Move(-1, 0);
                LockBlock();
            }
        }

        public void RotateBlockCW()
        {
            if (GameOver) return;
            CurrentBlock.RotateCW();
            if (!IsBlockInValidPosition())
            {
                CurrentBlock.RotateCCW();
            }
        }

        public void RotateBlockCCW()
        {
            if (GameOver) return;
            
            CurrentBlock.RotateCCW();
            if (!IsBlockInValidPosition())
            {
                CurrentBlock.RotateCW();
            }
        }
    }
}

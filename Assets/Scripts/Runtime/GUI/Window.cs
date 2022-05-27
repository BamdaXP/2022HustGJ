using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/** <summary>窗体组件</summary> */
[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public sealed class Window : MonoBehaviour
{
    /** <summary>动画参数</summary> */
    [Serializable]
    public struct EaseOptions
    {
        /** <summary>动画持续时间</summary> */
        public float duration;
        /** <summary>动画曲线样式</summary> */
        public Ease ease;

        /** <summary>是否变换透明度</summary> */
        public bool fade;
        /** <summary>显示时的透明度</summary> */
        public float alphaOnShow;
        /** <summary>隐藏时的透明度</summary> */
        public float alphaOnHide;

        /** <summary>是否变换大小</summary> */
        public bool scale;
        /** <summary>显示时的大小</summary> */
        public Vector3 scaleOnShow;
        /** <summary>隐藏时的大小</summary> */
        public Vector3 scaleOnHide;

        /** <summary>是否变换局部坐标</summary> */
        public bool move;
        /** <summary>显示时的局部坐标</summary> */
        public Vector3 posOnShow;
        /** <summary>隐藏时的局部坐标</summary> */
        public Vector3 posOnHide;

        /** <summary>动画参数是否有效(只读)</summary> */
        public bool IsValid
        {
            get
            {
                if (fade)
                    return true;
                if (scale)
                    return true;
                if (move)
                    return true;
                return false;
            }
        }

        /** <summary>默认动画参数(只读)</summary> */
        public static readonly EaseOptions Default = new EaseOptions()
        {
            duration = 0.5f,
            ease = DG.Tweening.Ease.InOutQuad,

            fade = true,
            alphaOnShow = 1.0f,
            alphaOnHide = 0.0f,

            scale = false,
            move = false
        };

        /**
         * <summary>拷贝构造函数</summary>
         * <param name="ref">要复制的动画参数</param>
         */
        public EaseOptions(EaseOptions @ref)
        {
            duration = @ref.duration;
            ease = @ref.ease;

            fade = @ref.fade;
            alphaOnShow = @ref.alphaOnShow;
            alphaOnHide = @ref.alphaOnHide;

            scale = @ref.scale;
            scaleOnShow = @ref.scaleOnShow;
            scaleOnHide = @ref.scaleOnHide;

            move = @ref.move;
            posOnShow = @ref.posOnShow;
            posOnHide = @ref.posOnHide;
        }

        /**
         * <summary>设置透明度变换参数</summary>
         * <param name="fade">是否变换透明度</param>
         * <param name="alphaOnShow">显示时的透明度</param>
         * <param name="alphaOnHide">隐藏时的透明度</param>
         * <returns>动画参数本身</returns>
         */
        public EaseOptions Fade(bool fade, float alphaOnShow, float alphaOnHide)
        {
            this.fade = fade;
            this.alphaOnShow = alphaOnShow;
            this.alphaOnHide = alphaOnHide;
            return this;
        }
        /**
         * <summary>设置大小变换参数</summary>
         * <param name="scale">是否变换大小</param>
         * <param name="scaleOnShow">显示时的大小</param>
         * <param name="scaleOnHide">隐藏时的大小</param>
         * <returns>动画参数本身</returns>
         */
        public EaseOptions Scale(bool scale, Vector3 scaleOnShow, Vector3 scaleOnHide)
        {
            this.scale = scale;
            this.scaleOnShow = scaleOnShow;
            this.scaleOnHide = scaleOnHide;
            return this;
        }
        /**
         * <summary>设置坐标变换参数</summary>
         * <param name="move">是否变换坐标</param>
         * <param name="posOnShow">显示时的坐标</param>
         * <param name="posOnHide">隐藏时的坐标</param>
         * <returns>动画参数本身</returns>
         */
        public EaseOptions Move(bool move, Vector3 posOnShow, Vector3 posOnHide)
        {
            this.move = move;
            this.posOnShow = posOnShow;
            this.posOnHide = posOnHide;
            return this;
        }
        /**
         * <summary>设置基本动画参数</summary>
         * <param name="duration">动画时长</param>
         * <param name="ease">动画插值曲线样式</param>
         * <returns>动画参数本身</returns>
         */
        public EaseOptions Ease(Ease ease, float duration = 1.0f)
        {
            this.ease = ease;
            this.duration = duration;
            return this;
        }
    }

    /** <summary>窗体当前是否可见(只读)</summary> */
    public bool Visible { get; private set; }
    public bool IsAnimating = false;

    public EaseOptions options = EaseOptions.Default;
    [SerializeField]
    [Tooltip("开始时是否显示")]
    private bool _showOnLoad;

    [SerializeField]
    [Tooltip("窗体名称")]
    private string windowName = "";

    private CanvasGroup _canvasGroup;

    private static readonly Dictionary<string, Window> windows = new Dictionary<string, Window>();

    private Tweener tweener;

    public bool Show(Action<Window> onShow = null)
    {
        return Show(options, onShow);
    }
    /**
     * <summary>尝试显示窗体</summary>
     * <param name="options">动画参数</param>
     * <param name="onShow">显示完成时的回调</param>
     * <returns>是否显示成功</returns>
     */
    public bool Show(EaseOptions options, Action<Window> onShow = null)
    {
        if (IsAnimating)
            Complete();
        if (Visible)
            return false;
        if (options.IsValid)
        {
            IsAnimating = true;
            var progress = 0.0f;
            tweener = DOTween.To(() => progress, x => progress = x, 1.0f, options.duration)
                .SetEase(options.ease)
                .OnUpdate(() =>
                {
                    if (options.fade)
                        _canvasGroup.alpha = Mathf.Lerp(options.alphaOnHide, options.alphaOnShow, progress);
                    if (options.scale)
                        transform.localScale = Vector3.LerpUnclamped(options.scaleOnHide, options.scaleOnShow, progress);
                    if (options.move)
                        transform.localPosition = Vector3.LerpUnclamped(options.posOnHide, options.posOnShow, progress);
                })
                .OnComplete(() =>
                {
                    Visible = true;
                    IsAnimating = false;
                    _canvasGroup.blocksRaycasts = true;
                    onShow?.Invoke(this);
                })
                .Play();
        }
        else
        {
            Visible = true;
            IsAnimating = false;
            Refresh();
            onShow?.Invoke(this);
        }
        return true;
    }

    public bool Hide(Action<Window> onHide = null)
    {
        return Hide(options, onHide);
    }
    /**
     * <summary>尝试隐藏窗体</summary>
     * <param name="options">动画参数</param>
     * <param name="onHide">隐藏完成时的回调</param>
     * <returns>是否隐藏成功</returns>
     */
    public bool Hide(EaseOptions options, Action<Window> onHide = null)
    {
        if (IsAnimating)
            Complete();
        if (!Visible)
            return false;
        if (options.IsValid)
        {
            IsAnimating = true;
            var progress = 1.0f;
            tweener = DOTween.To(() => progress, x => progress = x, 0.0f, options.duration)
                .SetEase(options.ease)
                .OnUpdate(() =>
                {
                    if (options.fade)
                        _canvasGroup.alpha = Mathf.Lerp(options.alphaOnHide, options.alphaOnShow, progress);
                    if (options.scale)
                        transform.localScale = Vector3.LerpUnclamped(options.scaleOnHide, options.scaleOnShow, progress);
                    if (options.move)
                        transform.localPosition = Vector3.LerpUnclamped(options.posOnHide, options.posOnShow, progress);
                })
                .OnComplete(() =>
                {
                    Visible = false;
                    IsAnimating = false;
                    _canvasGroup.blocksRaycasts = false;
                    onHide?.Invoke(this);
                })
                .Play();
        }
        else
        {
            Visible = false;
            IsAnimating = false;
            Refresh();
            onHide?.Invoke(this);
        }
        return true;
    }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        windows[windowName] = this;
    }
    private void Start()
    {
        Visible = _showOnLoad;
        Refresh();
    }
    private void OnValidate()
    {
        Visible = _showOnLoad;
        Refresh();
    }

    private void Refresh()
    {
        if (!_canvasGroup)
            _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = Visible ? 1.0f : 0.0f;
        _canvasGroup.blocksRaycasts = Visible;
    }

    /**
     * <summary>尝试显示窗体</summary>
     * <param name="windowName">要显示的窗体名称</param>
     * <param name="options">动画参数</param>
     * <param name="onShow">显示完成时的回调</param>
     * <returns>是否显示成功</returns>
     */
    public static bool Show(string windowName, EaseOptions options, Action<Window> onShow = null)
    {
        if (windows.TryGetValue(windowName, out Window window))
            return window.Show(options, onShow);
        return false;
    }
    /**
     * <summary>尝试隐藏窗体</summary>
     * <param name="windowName">要隐藏的窗体名称</param>
     * <param name="options">动画参数</param>
     * <param name="onHide">隐藏完成时的回调</param>
     * <returns>是否隐藏成功</returns>
     */
    public static bool Hide(string windowName, EaseOptions options, Action<Window> onHide = null)
    {
        if (windows.TryGetValue(windowName, out Window window))
            return window.Hide(options, onHide);
        return false;
    }

    public static Window GetWindow(string windowName)
    {
        Window window;
        if (windows.TryGetValue(windowName, out window))
            return window;
        else
            return null;
    }
    public static void Show(string windowName)
    {
        Show(windowName, EaseOptions.Default);
    }
    public static void Hide(string windowName)
    {
        Hide(windowName, EaseOptions.Default);
    }

    public void Complete()
    {
        tweener.Kill(true);
        IsAnimating = false;
    }
}
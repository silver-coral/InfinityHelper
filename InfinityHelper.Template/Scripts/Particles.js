(function ($) {
    $.fn.extend({
        "particle": function (options) {
            _this = this;

            if (!isValid(options) || $.isMobile() || !$.isHtml5()) {
                return _this;
            }
            opts = $.extend({}, defaluts, options);

            initCanvas();
            initParticles();
            bindEvent();
            $.drawAnimation(60, render);

            return _this;
        }
    });

    var _this;
    var particles;
    var canvas;
    var ctx;
    var opts;
    //var framesSpeed = 3;
    //var frames = 0;

    //默认参数
    var defaluts = {
        number: 30,
        lineWidth: 1,
        shadowBlur: 20,
        shadowColor: "#fff",
        stokeStyle: "#888",
        fillStyle: "#222"
    };

    //检测参数是否合法
    function isValid(options) {
        return !options || (options && typeof options === "object") ? true : false;
    }

    function initCanvas() {
        var cvsObj = $("<canvas class='bg-canvas'></canvas>");
        _this.append(cvsObj);
        canvas = cvsObj[0];
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
        ctx = canvas.getContext('2d');
    };

    function initParticles() {
        particles = [];
        for (var i = 0; i < opts.number; i++) {
            particles.push(new Particle());
        }
    };

    function drawLine(p1, p2, o) {
        ctx.beginPath();
        ctx.moveTo(p1.x, p1.y);
        ctx.lineTo(p2.x, p2.y);

        ctx.save();
        ctx.globalAlpha = o;
        ctx.lineWidth = opts.lineWidth;
        ctx.strokeStyle = opts.stokeStyle;
        ctx.stroke();
        ctx.restore();
    };

    function bindEvent() {
        $(window).resize(function () {
            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight;
        });

        $(canvas).click(function (e) {
            var mouseParticle = new Particle(e.clientX, e.clientY);
            particles.push(mouseParticle);
        });
    }

    function render() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        for (var i = 0; i < particles.length; i++) {
            for (var j = 0; j < i; j++) {
                var A = Math.abs(particles[j].x - particles[i].x),
                    B = Math.abs(particles[j].y - particles[i].y);
                var lineLength = Math.sqrt(A * A + B * B);
                var C = 1 / lineLength * 7 - 0.009;
                var lineOpacity = C > 0.1 ? 0.1 : C;
                if (lineOpacity > 0.025) {
                    drawLine(particles[i], particles[j], lineOpacity * 7);
                }
            }
        }
        for (var i = 0; i < particles.length; i++) {
            var p = particles[i];
            p.move();
            p.render();
        }
    };

    var Particle = function (_x, _y) {
        if (_x && _y) {
            this.x = _x;
            this.y = _y;
        }
        else {
            this.x = Math.random() * canvas.width;
            this.y = Math.random() * canvas.height;
        }

        this.r = 4 + Math.random() * 6;
        this.vx = (Math.random() * 1 - 0.5);
        this.vy = (Math.random() * 1 - 0.5);
        this.vr = (Math.random() * 0.1 - 0.05);
    }

    Particle.prototype = {
        render: function () {
            if (this.r > 1) {
                ctx.beginPath();
                ctx.arc(this.x, this.y, this.r, 0, 2 * Math.PI);

                ctx.save();
                ctx.shadowBlur = opts.shadowBlur;
                ctx.shadowColor = opts.shadowColor;
                ctx.fillStyle = opts.fillStyle;
                ctx.fill();
                ctx.restore();
            }
        },
        move: function () {
            this.x += this.vx;
            this.y += this.vy;
            this.r += this.vr;
            if (this.x > canvas.width) {
                this.x = 0;
            }
            else if (this.x < 0) {
                this.x = canvas.width;
            }
            if (this.y > canvas.height) {
                this.y = 0;
            }
            else if (this.y < 0) {
                this.y = canvas.height;
            }
            if (this.r < 4 || this.r > 10) {
                this.vr *= -1;
            }
        }
    }
})(window.jQuery);
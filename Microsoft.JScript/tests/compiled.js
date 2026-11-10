// Scanner / parser / compiler integration script.
function check(c, m) { if (!c) throw new Error(m); }

// Comments and line handling.
check(1 /* inline */ + 2 === 3, 'comments');
/* multi
   line
   comment */
check(2 === 2 /* trailing */, 'block');
check(1 // line comment
      === 1, 'line comment');

// Automatic semicolon insertion.
function asiReturn() { return
  1; }
check(asiReturn() === undefined, 'asi return');
var aa = 1
var bb = 2
check(aa + bb === 3, 'asi vars');

// Operators and coercion.
check(1 + 2 * 3 === 7, 'precedence');
check('5' - 2 === 3, 'coerce');
check(1 == '1' && !(1 === '1'), 'equality');
check((0 ? 'a' : 1 ? 'b' : 'c') === 'b', 'ternary');
check((3 & 1) === 1 && (2 | 1) === 3 && (5 ^ 1) === 4, 'bitwise');
check((1 << 4) === 16 && (-1 >>> 0) === 4294967295, 'shift');
check(typeof notDefinedVarOrFunction === 'undefined', 'typeof');
check(delete ({x:1}).x === true, 'delete');

// Control flow.
var s = '';
for (var i = 0; i < 5; i++) { if (i === 2) continue; s += i; }
check(s === '0134', 'for continue');
s = '';
outer: for (var i = 0; i < 3; i++) { for (var j = 0; j < 3; j++) { if (j === 1) continue outer; s += '' + i + j; } }
check(s === '001020', 'labeled continue');
s = 0;
do { s++; } while (s < 3);
check(s === 3, 'do while');
s = '';
switch (2) { case 1: s = 'a'; break; case 2: s = 'b'; case 3: s += 'c'; break; default: s = 'd'; }
check(s === 'bc', 'switch fallthrough');
s = '';
with ({ q: 5 }) { s = q; }
check(s === 5, 'with');
s = '';
try { s += 't'; throw new Error('x'); } catch (e) { s += 'c' + e.message; } finally { s += 'f'; }
check(s === 'tcxf', 'try');

// Functions, scopes, closures.
function add(a, b) { return a + b; }
check(add(2, 3) === 5, 'function');
function fact(n) { return n <= 1 ? 1 : n * fact(n - 1); }
check(fact(6) === 720, 'recursion');
var counter = (function () { var n = 0; return function () { return ++n; }; })();
check(counter() === 1 && counter() === 2, 'closure');
var fExpr = function (x) { return x * 2; };
check(fExpr(4) === 8, 'function expression');
var outerVar = 10;
function readsOuter() { return outerVar; }
check(readsOuter() === 10, 'outer scope');
check((function () { return this != null; })() === true, 'this');

// Objects and prototypes.
var o = { a: 1, b: 'x', 'c d': 3 };
check(o.a === 1 && o['b'] === 'x' && o['c d'] === 3, 'object literal');
function Point(x, y) { this.x = x; this.y = y; }
Point.prototype.sum = function () { return this.x + this.y; };
var p = new Point(2, 3);
check(p.sum() === 5, 'prototype');
check(p instanceof Point, 'instanceof');
check('x' in p && !('z' in p), 'in');
check(p.constructor === Point, 'constructor');

// Arrays.
var arr = [1, 2, 3];
check(arr.length === 3 && arr.join('-') === '1-2-3', 'array');
arr.push(4);
check(arr.pop() === 4 && arr.length === 3, 'push pop');
check(arr.slice(1).join(',') === '2,3', 'slice');
check(arr.splice(1, 1, 9).join(',') === '2' && arr.join(',') === '1,9,3', 'splice');
check([3, 1, 2].sort().join(',') === '1,2,3', 'sort');
check([1, 2, 3].reverse().join(',') === '3,2,1', 'reverse');
check([1, 2].concat([3]).join(',') === '1,2,3', 'concat');
check([1, 2, 3].indexOf(2) === 1, 'indexOf');
check(String([1, [2, 3]]) === '1,2,3', 'array toString');

// Strings.
check('abc'.charAt(1) === 'b' && 'abc'.charCodeAt(0) === 97, 'char');
check('abcdef'.substring(1, 3) === 'bc', 'substring');
check('abcdef'.substr(2, 2) === 'cd', 'substr');
check('abcdef'.slice(-2) === 'ef', 'string slice');
check('abc'.toUpperCase() === 'ABC' && 'ABC'.toLowerCase() === 'abc', 'case');
check('a-b-c'.split('-').join('+') === 'a+b+c', 'split');
check('abc'.replace(/b/, 'X') === 'aXc', 'replace');
check('abcabc'.replace(/b/g, 'X') === 'aXcaXc', 'replace global');
check(/ab+c/i.test('ABBBC'), 'regex test');
check('abcabc'.match(/b/g).length === 2, 'match');
check('abc'.search(/b/) === 1, 'search');
check(String.fromCharCode(65, 66) === 'AB', 'fromCharCode');
check(' a '.length === 3, 'string length');
check('x'.concat('y', 'z') === 'xyz', 'concat string');

// Numbers and Math.
check(parseInt('10px') === 10 && parseInt('ff', 16) === 255, 'parseInt');
check(parseFloat('3.5e2') === 350, 'parseFloat');
check(isNaN(Number('x')) && isFinite(3), 'isNaN');
check(Math.max(3, 7, 2) === 7 && Math.min(3, 7, 2) === 2, 'min max');
check(Math.abs(-5) === 5 && Math.ceil(1.1) === 2 && Math.floor(1.9) === 1, 'math');
check(Math.pow(2, 10) === 1024 && Math.sqrt(16) === 4, 'pow sqrt');
check((255).toString(16) === 'ff', 'radix');
check((1.25).toFixed(1) === '1.3' || (1.25).toFixed(1) === '1.2', 'toFixed');

// Dates.
var date = new Date(2000, 0, 1, 12, 30, 15);
check(date.getFullYear() === 2000 && date.getMonth() === 0 && date.getDate() === 1, 'date parts');
check(date.getHours() === 12 && date.getMinutes() === 30 && date.getSeconds() === 15, 'time parts');
var d2 = new Date(0); d2.setUTCFullYear(2001);
check(d2.getUTCFullYear() === 2001, 'date set');

// Errors.
check(new Error('m').message === 'm', 'error');
check(new TypeError('m') instanceof Error, 'error type');
try { null.foo; } catch (e) { check(e instanceof TypeError || e instanceof Error, 'runtime error'); }

// eval.
check(eval('1 + 2') === 3, 'eval');
check(eval('var ev = 4; ev') === 4, 'eval var');
function evalInFn() { var local = 7; return eval('local'); }
check(evalInFn() === 7, 'eval local');

// JScript .NET extensions.
var typed : int = 5;
check(typed === 5, 'typed var');
function typedFn(x : int) : int { return x + 1; }
check(typedFn(4) === 5, 'typed function');
const KC : int = 9;
check(KC === 9, 'const');
enum Color { Red, Green, Blue }
check(Color.Green === 1, 'enum');
class Box {
    var v : int;
    function Box(x : int) { this.v = x; }
    function getV() : int { return this.v; }
}
var box : Box = new Box(11);
check(box.getV() === 11, 'class');
package Pkg { var q = 3; }
check(Pkg.q === 3, 'package');
import System;
check(System.Math.Max(1, 2) === 2, 'import');

// Conditional compilation.
/*@cc_on @if (@_jscript) var ccValue = 1; @else var ccValue = 2; @end @*/
check(ccValue === 1, 'cc if');
@set @i = 0 @while (@i < 3) { @set @i++ }
check(@i === 3, 'cc while');

// Position directive.
//@position(file='virtual.js', line=1, column=0, end)
check(true, 'position directive');
